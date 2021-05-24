using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game.Net {
	public class USocket
	{
		UdpClient udpClient;

		string ip = "10.235.102.238";
		int port = 8888;

		public static IPEndPoint serverAddr;
		public static UClient local;

		public USocket(Action<BufferEntity> dispathNetEvent) {
			udpClient = new UdpClient(0);
			serverAddr = new IPEndPoint(IPAddress.Parse(ip), port);
			local = new UClient(this, serverAddr, 0, 0, 0, dispathNetEvent);
			ReceiveTask();
		}

		ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
		private async void ReceiveTask()
		{
			while (udpClient != null) {
				try
				{
					UdpReceiveResult result = await udpClient.ReceiveAsync();
					awaitHandle.Enqueue(result);
					Debug.Log("接收到了消息");
				}
				catch (Exception e) {
					Debug.LogError($"ReceiveTask exception: {e.Message}");
				}
			}
		}

		// 发送报文的接口
		public async void Send(byte[] data, IPEndPoint endPoint) {
			if (udpClient == null) {
				return;
			}

			try
			{
				int length = await udpClient.SendAsync(data, data.Length, serverAddr);
				Debug.Log($"发送消息了: {length}");
			}
			catch (Exception e) {
				Debug.LogError($"发送异常: {e.Message}");
			}
		}

		// 发送ACK 报文，通知对方已经收到消息， 确认到达
		public void SendAck(BufferEntity data) {
			Send(data.buffer, serverAddr);
		}



		/// <summary>
		/// // Update 去进行调用
		/// </summary>
		public void Handle() {
			if (awaitHandle.Count <= 0) {
				return;
			}

			UdpReceiveResult data;
			if (awaitHandle.TryDequeue(out data)) {
				BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
				if (bufferEntity.isFull) {
					Debug.Log($"处理消息, id:{bufferEntity.messageID}, 序号: {bufferEntity.sn}");
					// 业务处理
					local.HandleMessage(bufferEntity);
				}
			}
		}

		/// <summary>
		/// 关闭udpClient
		/// </summary>
		public void Close() {
			if (local != null) {
				local = null;
			}
			if (udpClient != null) {
				udpClient.Close();
				udpClient = null;
			}
		}
	}

}

