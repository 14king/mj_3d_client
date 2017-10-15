using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NetWork{
	public class NetClient{
		private Socket socket;
		bool _connected = false;
		private int _head_len = 0;
		private int _body_len = 0;
		public byte[] _buff = new Byte[1024];
		public int _buff_pos = 0;
		private List<JObject> msgList = new List<JObject>();

		private static NetClient _netClient;

		private NetClient(){
		}

		public static NetClient Instance(){
			if (_netClient == null) {
				_netClient = new NetClient ();
			}

			return _netClient;
		}

		public bool Connect( string ip, int port){
			try {
				Debug.Log ("begin connect " + ip);
				socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
				socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallBack), null);
				return true ;
			}
			catch {
				return false ;
			}
		}

		public void ConnectCallBack(IAsyncResult ar){
			try {
				socket.EndConnect(ar);  
				_connected = true; 
				BeginReceive ();
			}catch(Exception e){
				Debug.Log (e.ToString());
			}
		}

		public void Disconnect(){
			socket.Close();
		}

        public void Close()
        {
            _connected = false;
            socket.Close();
        }

		public void BeginReceive(){
			try{
				int max = 2;
				if(_head_len == 2){
					max = _body_len;
				}
				socket.BeginReceive(_buff, _buff_pos, max - _buff_pos, 0, new AsyncCallback(ReceiveCallback), socket);
			}
			catch(Exception e) {
				Debug.Log (e.ToString ());
			}
		}

		private void ReceiveCallback(IAsyncResult ar){
			try{
				int bytesRead = socket.EndReceive(ar);
				Debug.Log(String.Format("recv {0}",bytesRead));
				if (bytesRead <= 0) {
					Close();
					return;
				}

				// 包头
				if(_body_len == 0){
					_head_len += bytesRead;
					_buff_pos += bytesRead;
					if(_head_len<2){
						BeginReceive();
					}
					_body_len = (_buff[0] * 256 + _buff[1]);
					_buff_pos = 0;
					BeginReceive();
					return;
				}

				// 包体没有接收全
				if(_buff_pos < _body_len) {
					BeginReceive();
					return;
				}

				// 包体接收完毕
				Debug.Log(string.Format("packet len:{0}, body_len:{1}", _body_len+2, _body_len));
				string str = System.Text.Encoding.Default.GetString(_buff, 0, _body_len);
                JObject obj = JObject.Parse(str);
				msgList.Add(obj);

                _head_len = 0;
                _body_len = 0;
                _buff_pos = 0;
                BeginReceive();
            }
			catch(Exception e){
				Debug.Log (e.ToString ());
                Close();
                return;
			}
		}

		public JObject PeekMsg(){
			if (msgList.Count == 0) {
				return null;
			}

			JObject msg = msgList[0];
			msgList.RemoveAt (0);
			return msg;
		}

		public void WriteMsg(JObject obj)
        {
            try{
                string str = obj.ToString();
                byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(str);
               	UInt16 nlen = (UInt16)byteArray.Length;
				byte[] packBuff = new byte[nlen + 2];

                byte[] b = BitConverter.GetBytes(nlen);
                packBuff[0] = b[1];
                packBuff[1] = b[0];
                Buffer.BlockCopy(byteArray, 0, packBuff, 2, nlen);
                socket.BeginSend(packBuff, 0, packBuff.Length, 0, new AsyncCallback(SendCallback), packBuff);
            }
            catch(Exception e) {
				Debug.Log (e.ToString());
			}
		}

		private void SendCallback(IAsyncResult ar)  
		{  
		} 

		public void ToHexString(byte[] bytes)
		{
			try{
				string byteStr = string.Empty;
				if (bytes != null || bytes.Length > 0)
				{
					foreach (var item in bytes)
					{
						byteStr += string.Format("{0:X2} ", item);
					}
				}
				Debug.Log(byteStr);
			}catch(Exception e){
				Debug.Log (e.ToString ());
			}
		}
	}
} 
