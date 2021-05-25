using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MysqlTest
{
	class Program
	{
		static void Main(string[] args)
		{
			string connStr = "database=test;datasource=10.235.102.249;port=3306;user=root;pwd=root";
			MySqlConnection conn = new MySqlConnection(connStr);

			conn.Open();

			#region 查询
			//MySqlCommand cmd = new MySqlCommand("select * from user", conn);
			//MySqlDataReader reader =  cmd.ExecuteReader();

			//while (reader.Read()) {
			//	string userName = reader.GetString("username");
			//	string password = reader.GetString("password");
			//	Console.WriteLine(userName + ":" + password);
			//}

			//reader.Close();
			#endregion

			#region 插入
			//string userName1 = "zjw4";
			//string password1 = "123456";
			////MySqlCommand cmd1 = new MySqlCommand("insert into user set username='" + userName1 + "',`password`='" + password1 + "'", conn);

			//// 防止sql 注入攻击
			//MySqlCommand cmd1 = new MySqlCommand("insert into user set username=@un, password=@pwd", conn);
			//cmd1.Parameters.AddWithValue("un", userName1);
			//cmd1.Parameters.AddWithValue("pwd", password1);

			//int rows  = cmd1.ExecuteNonQuery();
			//if (rows > 0) {
			//	Console.WriteLine("insert success");
			//}
			#endregion

			#region 删除
			//MySqlCommand cmd = new MySqlCommand("delete from user where id=@id", conn);
			//cmd.Parameters.AddWithValue("id", 1);
			//int effectedRows = cmd.ExecuteNonQuery();
			//if (effectedRows > 0) {
			//	Console.WriteLine("insert success");
			//}
			#endregion

			#region 更新
			//MySqlCommand cmd = new MySqlCommand("update user set password= @pwd where id = @id", conn);
			//cmd.Parameters.AddWithValue("pwd", "123456789");
			//cmd.Parameters.AddWithValue("id", 2);

			//int effectedRows = cmd.ExecuteNonQuery();
			//if (effectedRows > 0) {
			//	Console.WriteLine("update success");
			//}
			#endregion

			conn.Close();
		}
	}
}
