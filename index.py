__author__ = 'boddmg'
import tornado.ioloop
import tornado.web
import tornado.websocket
import os
import sys
from socket import *

sys.path.append(os.path.dirname(__file__))

websocket_connections = set()
udpCliSock = socket(AF_INET, SOCK_DGRAM)
DEST_HOST = "192.168.199.214"
DEST_PORT = 23333

class MainHandler(tornado.web.RequestHandler):
    def get(self):
        self.write(open("./index.html").read())

class WebSocket(tornado.websocket.WebSocketHandler):
    def open(self):
        websocket_connections.clear()
        websocket_connections.add(self)
        print str(self)+"opened"

    def on_message(self, message):
        #print(message)
        udpCliSock.sendto(message + "\n",(DEST_HOST,DEST_PORT))
        pass

    def send_message_safe(self,data):
        self.write_message(data)

    def on_close(self):
        websocket_connections.remove(self)
        print str(self)+"closed"

def main():
        settings = {
            "static_path": "./",
            'debug':True
        }
        application = tornado.web.Application([
            (r"/", MainHandler),
            (r"/websocket", WebSocket)
        ], **settings)
        application.listen(2333)
        tornado.ioloop.IOLoop.instance().start()
        # @tornado.ioloop.IOLoop.instance().add_callback(i.send_message_safe,pure_json_data)

if __name__ == '__main__':
    main()
