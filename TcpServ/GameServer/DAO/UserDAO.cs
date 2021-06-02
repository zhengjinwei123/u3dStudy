﻿using System;
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


		public bool GetUserByUsername(MySqlConnection conn, string username)
		{
			MySqlDataReader reader = null;
			try
			{
				MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", conn);
				cmd.Parameters.AddWithValue("username", username);
				reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);
			}
			finally
			{
				if (reader != null) reader.Close();
			}
			return false;
		}

		public void AddUser(MySqlConnection conn, string username, string password)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand("insert into user set username = @username , password = @password", conn);
				cmd.Parameters.AddWithValue("username", username);
				cmd.Parameters.AddWithValue("password", password);
				cmd.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine("在AddUser的时候出现异常：" + e);
			}
		}
	}
}
