using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server
{
	class Server
	{
		private IPEndPoint ipEndPoint;
		private Socket serverSocket;


		public Server() {

		}

		public Server(string ipStr, int port) {
			SetIpAndPort(ipStr, port);
		}

		public void SetIpAndPort(string ipStr, int port) {
			ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
		}

		public void Start() {
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			serverSocket.Bind(ipEndPoint);
			serverSocket.Listen(64);
			serverSocket.BeginAccept(AcceptCallback, null);
		}

		private void AcceptCallback(IAsyncResult ar) {
			Socket clientSocket = serverSocket.EndAccept(ar);


		}


	}
}
