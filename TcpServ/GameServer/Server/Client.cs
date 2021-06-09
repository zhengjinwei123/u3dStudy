using Common;
using GameServer.Model;
using GameServer.Tool;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Servers
{
	class Client
	{
		private Socket clientSocket;
		private Server server;
		private Message msg = new Message();
		private MySqlConnection mysqlConn;

		private Result result;
		private User user;
		private Room room;

		public MySqlConnection MysqlConn {
			get { return mysqlConn;  }
		}

		public int HP {
			get; set;
		}

		public bool IsDie() {
			return HP <= 0;
		}

		public bool TakeDamage(int damage) {
			HP -= damage;
			HP = Math.Max(HP, 0);
			if (HP <= 0) return true;

			return false;
		}

		public Room Room {
			set { room = value; }
			get { return room;  }
		}

		public Client() {

		}

		public Client(Socket socket, Server server) {
			this.clientSocket = socket;
			this.server = server;

			this.mysqlConn = ConnHelper.Connect();
		}

		public void SetUserData(User user, Result result) {
			this.user = user;
			this.result = result;
		}

		public void Start() {
			if (clientSocket == null || clientSocket.Connected == false) return;
			clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
		}

		private void ReceiveCallback(IAsyncResult ar) {

			try
			{
				if (clientSocket == null || clientSocket.Connected == false) {
					return;
				}

				int count = clientSocket.EndReceive(ar);
				if (count <= 0)
				{
					Close();
					return;
				}

				msg.ReadMessage(count, OnProcessMessage);

				Start();
			}
			catch (Exception e) {
				Console.WriteLine(e);
				Close();
			}
		}

		private void Close() {
			if (clientSocket != null) {
				clientSocket.Close();
				clientSocket = null;
			}

			if (room != null) {
				room.QuitRoom(this);
			}

			ConnHelper.CloseConnection(mysqlConn);

			server.RemoveClient(this);
		}

		private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data) {

			Console.WriteLine("请求来了:" + requestCode + ":" + actionCode + ":" + data);

			server.HandleRequest(requestCode, actionCode, data, this);
		}

		public void Send(ActionCode actionCode, string data) {

			try
			{
				byte[] bytes = Message.PackData(actionCode, data);
				clientSocket.Send(bytes);
			}
			catch (Exception e) {
				Console.WriteLine("无法发送消息:" + e);
			}
		}

		public string GetUserData() {
			return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
		}

		public int GetUserId() {
			return user.Id;
		}

		public bool IsHouseOwner() {
			return room.IsHouseOwner(this);
		}
	}
}
