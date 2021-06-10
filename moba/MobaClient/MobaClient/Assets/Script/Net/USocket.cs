using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 使用UdpClient 进行通信
/// 初始化-> 发送报文 -> 接收报文 -> 处理报文 -> 解包
/// </summary>
namespace Game.Net
{
	// 提供socket发送的接口 以及 socket 接收的业务
	public class USocket
	{
		UdpClient udpClient;

		string ip = "10.235.102.200"; // 服务器主机
		int port = 8899; // 服务器程序

		public static IPEndPoint server;
		public static UClient local; // 客户端代理: 完成发送的逻辑 处理的逻辑， 保证报文的顺序

		public USocket(Action<BufferEntity> dispatchNetEvent)
		{
			udpClient = new UdpClient();
			server = new IPEndPoint(IPAddress.Parse(ip), port);
			local = new UClient(this, server, 0, 0, 0, dispatchNetEvent);

			ReceiveTask(); // 接收消息
		}

		ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();

		/// <summary>
		/// 接收报文
		/// </summary>
		public async void ReceiveTask()
		{
			while (udpClient != null)
			{
				try
				{
					UdpReceiveResult result = await udpClient.ReceiveAsync();
					awaitHandle.Enqueue(result);
					Debug.Log($"接收到服务器的消息");

				}
				catch (Exception e)
				{
					Debug.LogError(e.Message);
				}
			}
		}

		/// <summary>
		/// 发送报文
		/// </summary>
		/// <param name="data"></param>
		/// <param name="endPont"></param>
		public async void Send(byte[] data, IPEndPoint endPont) {
			if (udpClient != null)
			{
				try
				{
					int length = await udpClient.SendAsync(data, data.Length, ip, port);
					Debug.Log($"发送消息 byte: {length} {ip} {port}");
				}
				catch (Exception e)
				{

					Debug.LogError($"发送异常:{e.Message}");
				}
			}
			else {
				Debug.Log("发送失败， 没有建立连接");
			}
		}

		/// <summary>
		/// 发送 ACK 报文， 解包后马上调用
		/// </summary>
		/// <param name="bufferEntity"></param>
		public void SendACK(BufferEntity bufferEntity) {
			Send(bufferEntity.buffer, server);
		}

		// update 里面进行调用
		public void Handle() {
			if (awaitHandle.Count > 0) {

				UdpReceiveResult data;
				if (awaitHandle.TryDequeue(out data)) {
					// 反序列化
					BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
					if (bufferEntity.isFull) {
						// 处理业务
						Debug.Log($"处理消息,id: {bufferEntity.messageID}, 序号: {bufferEntity.sn}");
						local.Handle(bufferEntity);
					}
				}
			}
		}

		/// <summary>
		/// 关闭udpClient
		/// </summary>
		public void Close() {
			if (udpClient != null) {
				udpClient.Close();
				udpClient = null;
			}

			if (local != null) {
				local.OnDisconnect();
				local = null;
			}
		}
	}
}

