using MobaServer.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Debug.Log("启动服务器...");
			NetSystemInit();

			Console.ReadLine();
		}

		public static USocket uSocket;
		static void NetSystemInit() {
			uSocket = new USocket(DispatchNetEvent);
			Debug.Log("网络系统初始化完成");

		}

		static void DispatchNetEvent(BufferEntity buffer) {
			// 进行报文分发
		}
	}
}
