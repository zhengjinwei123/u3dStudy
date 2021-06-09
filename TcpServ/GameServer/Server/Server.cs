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
		private List<Room> roomList = new List<Room>();

		private ControllerManager controllerManager;

		public Server() {
		}

		public Server(string ipStr, int port) {
			SetIpAndPort(ipStr, port);
			InitController();
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

			Console.WriteLine("客户端连接来了");

			clientList.Add(client);

			serverSocket.BeginAccept(AcceptCallback, null);
		}

		public void RemoveClient(Client client) {

			lock (clientList)
			{
				Console.WriteLine("关闭客户端连接");
				clientList.Remove(client);
			}
		}

		public void SendResponse(Client client, ActionCode actionCode, string data) {
			client.Send(actionCode, data);
		}

		public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client) {
			controllerManager.HandleRequest(requestCode, actionCode, data, client);
		}

		public void CreateRoom(Client client) {
			Room room = new Room(this);
			room.AddClient(client);
			roomList.Add(room);
		}

		public void RemoveRoom(Room room) {
			if (roomList != null && room != null) {
				roomList.Remove(room);
			}
		}

		public List<Room> GetRoomList() {
			return roomList;
		}

		public Room GetRoomById(int id) {
			foreach (Room room in roomList) {
				if (room.GetId() == id) return room;
			}

			return null;
		}

	}
}
