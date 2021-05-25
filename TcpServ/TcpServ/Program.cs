using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TcpServ
{
	class Program
	{
		//static byte[] dataBuffer = new byte[1024];
		static Message msgBuffer = new Message();

		static void Main(string[] args)
		{
			StartServerAsync();
			Console.ReadKey();
		}

		static void StartServerAsync() {
			Socket servSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// 10.235.102.200
			IPAddress ipAddr = IPAddress.Parse("10.235.102.200");
			IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8061);
			servSocket.Bind(ipEndPoint);
			servSocket.Listen(32);

			//Socket clientSocket = servSocket.Accept();
			servSocket.BeginAccept(AcceptCallback, servSocket);

		}

		static void AcceptCallback(IAsyncResult ar) {

			Socket servSocket  = ar.AsyncState as Socket;
			Socket clientSocket = servSocket.EndAccept(ar);

			string msg = "Hello client ! 你好...";
			byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
			clientSocket.Send(data);

			clientSocket.BeginReceive(msgBuffer.Data, msgBuffer.StartIndex, msgBuffer.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);

			servSocket.BeginAccept(AcceptCallback, servSocket);
		}

	
		static void ReceiveCallback(IAsyncResult ar) {
			Socket clientSocket = null;

			try {
			   clientSocket = ar.AsyncState as Socket;
				int count = clientSocket.EndReceive(ar);

				if (count <= 0) {
					clientSocket.Close();
					return;
				}

				// 更新 startIndex
				msgBuffer.AddCount(count);
				msgBuffer.ReadMessage();

				//string msg = Encoding.UTF8.GetString(msgBuffer.Data, msgBuffer.StartIndex, count);
				//Console.WriteLine("从客户端接收到数据:" + count +":" + msg);

				clientSocket.BeginReceive(msgBuffer.Data, msgBuffer.StartIndex, msgBuffer.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);
			} catch (Exception e) {
				Console.WriteLine(e);

				if (clientSocket != null)
				{
					clientSocket.Close();
				}
			}
			finally {
				
			}
		}

		static void StartServerSync() {
			Socket servSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// 10.235.102.200
			IPAddress ipAddr = IPAddress.Parse("10.235.102.200");
			IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8061);
			servSocket.Bind(ipEndPoint);
			servSocket.Listen(32);

			Socket clientSocket = servSocket.Accept();
			string msg = "Hello client ! 你好...";
			byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
			clientSocket.Send(data);

			byte[] dataBuffer = new byte[1024];
			int count = clientSocket.Receive(dataBuffer);
			string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);

			Console.WriteLine(msgReceive);

			clientSocket.Close();
			servSocket.Close();

			Console.ReadKey();
		}
	}
}
