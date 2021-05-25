using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
	class ConnHelper
	{
		public const string CONNECTIONSTRING = "datasource=10.235.102.249;port=3306;database=test;user=root;password=root;";

		public static  MySqlConnection Connect()
		{
			MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);

			try
			{
				conn.Open();
				return conn;
			}
			catch (Exception e) {
				Console.WriteLine("Conn mysql exception:" + e);
				return null;
			}
		}

		public static void CloseConnection(MySqlConnection conn) {

			if (conn == null) {
				Console.WriteLine("mysql connection must not be null");
				return;
			}

			try
			{
				conn.Close();
			}
			catch (Exception e) {
				Console.WriteLine("mysql close exception:" + e);
			}
		}
	}
}
