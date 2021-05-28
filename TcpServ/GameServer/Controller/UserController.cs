using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.DAO;
using GameServer.Model;
using GameServer.Servers;

namespace GameServer.Controller
{
	class UserController :  BaseController
	{
		private UserDAO userDAO;
		public UserController() {
			requestCode = RequestCode.User;
			userDAO = new UserDAO();
		}

		public string Login(string data, Client client, Server server) {
			string[] strs = data.Split(',');

			User user = userDAO.VerifyUser(client.MysqlConn, strs[0], strs[1]);
			if (user == null) {
				return ReturnCode.Fail.ToString();
			}
		}
	}
}
