using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
	class UserDAO
	{
		public User VerifyUser(MySqlConnection conn, string username, string password) {

			MySqlDataReader reader = null;
			try
			{
				MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", conn);
				cmd.Parameters.AddWithValue("username", username);
				cmd.Parameters.AddWithValue("password", password);
				reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					return new User(reader.GetInt32("id"), reader.GetString("username"), reader.GetString("password"));
				}

				return null;
			}
			catch (Exception e)
			{
				Console.WriteLine("VerifyUser exception:" + e);
			}
			finally {
				if (reader != null) reader.Close();
			}
			return null;
		}
	}
}
