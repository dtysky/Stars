from socket import *
import random

udp = socket(AF_INET,SOCK_DGRAM)
addr = ('127.0.0.1',8888)

udp.sendto('',addr)
udp.close()
