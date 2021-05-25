using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Controller;

namespace GameServer.Servers
{
	class Server
	{
		private IPEndPoint ipEndPoint;
		private Socket serverSocket;
		private List<Client> clientList = new List<Client>();
		private ControllerManager controllerManager;

		public Server() {
		}

		public Server(string ipStr, int port) {
			SetIpAndPort(ipStr, port);
		}

		public void  InitController() {
			controllerManager = new ControllerManager(this);
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

			Client client = new Client(clientSocket, this);
			client.Start();
			clientList.Add(client);
		}

		public void RemoveClient(Client client) {

			lock (clientList)
			{
				clientList.Remove(client);
			}
		}

		public void SendResponse(Client client, RequestCode requestCode, string data) {
			client.Send(requestCode, data);
		}

		public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client) {
			controllerManager.HandleRequest(requestCode, actionCode, data, client);
		}
	}
}
