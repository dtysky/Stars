using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class SocketInstance {
	public Socket sock;
	private List<float[]> list;
	private static SocketInstance instance;
	
	public static SocketInstance GetInstance(){
		if (instance == null)
			instance = new SocketInstance ();
		return instance;
	}
	
	SocketInstance(){
		IPEndPoint ip = new IPEndPoint (IPAddress.Any, 23333);
		sock = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sock.Bind (ip);
		list = new List<float[]>();
		Debug.Log ("Listen 0.0.0.0:23333");
		
		Thread thread = new Thread (new ThreadStart (ReceiveSocket));
		thread.IsBackground = true;
		thread.Start ();
	}
	
	private void ReceiveSocket(){
		int recv;
		byte[] data = new byte[1024];
		IPEndPoint sender = new IPEndPoint (IPAddress.Any, 0);
		EndPoint Remote = (EndPoint)(sender);
		
		float[] d;

		while (true) {
			recv = sock.ReceiveFrom (data, 1024, SocketFlags.None, ref Remote);
			
			Debug.Log(list.Count.ToString());
			if (recv<=0 ){
				Close();
				break;
			}
			DataParser(Encoding.ASCII.GetString(data,0,recv));
			d = Get();
			//Debug.Log ("Recv: "+d["x"]+", "+d["y"]+", "+d["z"]+"["+list.Count+"]");
		}
		
	}
	
	private void DataParser(string recv){
		try{
			string[] spliteddata = new string[3];
			float[] data = {0,0,0};
			
			spliteddata = recv.Split (',');
			data[0] = float.Parse(spliteddata [0]);
			data[1] = float.Parse(spliteddata [1]);
			data[2] = float.Parse(spliteddata [2]);
			
			list.Add (data);
		}catch (Exception e)
		{
			Debug.Log("data parser error: " + e );
		}
	}

	private void Close(){
		if (sock != null)
			sock.Close ();
		sock = null;
	}
	
	public float[] Get(){
		float[] d = {0, 0, 0};
		if (list.Count > 1) {
			d = list [0];
			list.RemoveAt (0);
			return d;
		} else if (list.Count == 1) {
			d = list[0];
			return d;
		}
		return d;
	}
}