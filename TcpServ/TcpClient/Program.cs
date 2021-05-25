using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("10.235.102.200"),  8061);
			clientSocket.Connect(remotePoint);

			byte[] data = new byte[1024];
			int count = clientSocket.Receive(data);
			string msg = Encoding.UTF8.GetString(data, 0, count);
			Console.WriteLine(msg);

			//while (true) {
			//	string s = Console.ReadLine();

			//	if (s == "c") {
			//		break;
			//	}
			//	clientSocket.Send(Encoding.UTF8.GetBytes(s));
			//}

			// 黏包演示
			for (int i = 0; i < 100; i++) {
				//clientSocket.Send(Encoding.UTF8.GetBytes(i.ToString()));
				clientSocket.Send(Message.GetBytes(i.ToString()));
			}
		
			clientSocket.Close();
			Console.ReadKey();
		}
	}
}
