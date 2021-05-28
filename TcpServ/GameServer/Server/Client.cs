using Common;
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

		public MySqlConnection MysqlConn {
			get { return mysqlConn;  }
		}
		public Client() {

		}

		public Client(Socket socket, Server server) {
			this.clientSocket = socket;
			this.server = server;

			this.mysqlConn = ConnHelper.Connect();
		}

		public void Start() {
			if (clientSocket == null || clientSocket.Connected == false) return;
			clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
		}

		private void ReceiveCallback(IAsyncResult ar) {

			try
			{
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

			ConnHelper.CloseConnection(mysqlConn);

			server.RemoveClient(this);
		}

		private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data) {
			server.HandleResponse(requestCode, actionCode, data, this);
		}

		public void Send(ActionCode actionCode, string data) {
			byte[] bytes = Message.PackData(actionCode, data);
			clientSocket.Send(bytes);
		}
	}
}
