using Common;
using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Servers
{

	enum RoomState {
		WatingJoin,
		WatingBattle,
		Battle,
		End,
	}
	class Room
	{
		private const int MAX_HP = 200;
		private List<Client> clientRoom = new List<Client>();
		private RoomState state = RoomState.WatingJoin;
		private Server server;

		public Room(Server server) {
			this.server = server;
		}

		public bool IsWaitingJoin() {
			return state == RoomState.WatingJoin;
		}

		public void AddClient(Client client) {
			client.HP = MAX_HP;
			clientRoom.Add(client);
			client.Room = this;
			if (clientRoom.Count >= 2) {
				state = RoomState.WatingBattle;
			}
		}

		public void RemoveClient(Client client) {
			client.Room = null;
			clientRoom.Remove(client);

			if (clientRoom.Count >= 2)
			{
				state = RoomState.WatingBattle;
			}
			else {
				state = RoomState.WatingJoin;
			}
		}

		public string GetHouseOwnerData() {
			return clientRoom[0].GetUserData();
		}

		public int GetId() {
			if (clientRoom.Count > 0) {
				return clientRoom[0].GetUserId();
			}
			return -1;
		}

		internal void TakeDamage(int damage, Client client)
		{
			
		}

		public string GetRoomData() {
			StringBuilder sb = new StringBuilder();
			foreach (Client client in clientRoom) {
				sb.Append(client.GetUserData() + "|");
			}

			if (sb.Length > 0) {
				sb.Remove(sb.Length - 1, 1);
			}

			return sb.ToString();
		}

		public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data) {
			foreach (Client client in clientRoom) {
				if (client != excludeClient) {
					server.SendResponse(client, actionCode, data);
				}
			}
		}

		public bool IsHouseOwner(Client client) {
			return client == clientRoom[0];
		}

		public void QuitRoom(Client client) {
			if (client == clientRoom[0])
			{
				Close();
			}
			else {
				clientRoom.Remove(client);
			}
		}

		public void Close()
		{
			foreach (Client client in clientRoom) {
				client.Room = null;
			}
			server.RemoveRoom(this);
		}


		public void StartTimer() {
			new Thread(RunTimer).Start();
		}

		private void RunTimer()
		{
			Thread.Sleep(1000);
			for (int i = 3; i > 0; i--) {
				BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
				Thread.Sleep(1000);
			}
			BroadcastMessage(null, ActionCode.StartPlay, "r");
		}
	}
}
