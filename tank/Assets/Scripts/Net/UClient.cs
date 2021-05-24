using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net {
	public class UClient
	{
		public IPEndPoint endPoint;
		USocket uSocket; // 处理发送
		int overTime = 150; // 毫秒 超时处理时间间隔

		public int sessionID; // 会话ID
		public int sendSN; // 发送的序号
		public int handleSN; // 处理的序号

		Action<BufferEntity> handleAction;// 处理报文的函数

		// 还没收到ack报文的缓存
		ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>();

		// 缓存已经收到的但是错序的报文
		ConcurrentDictionary<int, BufferEntity> waitHandle = new ConcurrentDictionary<int, BufferEntity>();

		public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionID, Action<BufferEntity> dispatchNetEvent) {
			this.uSocket = uSocket;
			this.endPoint = endPoint;
			this.sessionID = sessionID;
			this.sendSN = sendSN;
			this.handleSN = handleSN;

			handleAction = dispatchNetEvent;

			CheckTimeOut(); // 超时重发
		}

		// 处理消息， 按照报文的序号进行顺序处理
		public void HandleMessage(BufferEntity buffer) {
			if (this.sessionID == 0 && buffer.sessionID != 0) {
				// 第一次收到消息
				Debug.Log($"服务器发送给我们的会话ID是: {buffer.sessionID}");
				this.sessionID = buffer.sessionID;
			}

			switch (buffer.messageType) {
				case (UInt16)MessageType.ACK:
					BufferEntity bufferEntity;
					if (sendPackage.TryRemove(buffer.sn, out bufferEntity)) {
						Debug.Log($"收到ACK报文，序号是:{buffer.sn}");
					}
					break;
				case (UInt16)MessageType.LOGIC:
					BufferEntity ackPackage = new BufferEntity(buffer);
					uSocket.SendAck(ackPackage);// 告诉服务器， 我已经收到了这个报文

					// 处理业务
					HandleLogicPackage(buffer);
					break;
			}
		}

		private void HandleLogicPackage(BufferEntity buffer)
		{
			// 已经处理过的重复的报文， 丢弃
			if (buffer.sn <= handleSN) {
				return;
			}

			// 收到错序的报文， 缓存
			if (buffer.sn - handleSN > 1) {
				if (waitHandle.TryAdd(buffer.sn, buffer)) {
					Debug.Log($"收到错序的报文: {buffer.sn}");
				}
				return;
			}

			// 更新已经处理的报文
			handleSN = buffer.sn;
			if (handleAction != null) {
				handleAction(buffer);
			}

			// 检查一下缓存的数据，有没有包含下一条可以处理的数据
			BufferEntity nextBuffer;
			if (waitHandle.TryRemove(handleSN + 1, out nextBuffer)) {
				HandleLogicPackage(nextBuffer);
			}
		}

		// 发送
		public void SendMessage(BufferEntity package) {
			package.time = TimeHelper.Now();
			sendSN += 1;
			package.sn = sendSN;
			package.Encoder(false);

			if (sessionID != 0)
			{
				sendPackage.TryAdd(sendSN, package);
			}

		
			uSocket.Send(package.buffer, endPoint);
		}

		private async void CheckTimeOut()
		{
			await Task.Delay(overTime);
			foreach (var pack in sendPackage.Values) {
				if (pack.recurCount >= 10) {
					Debug.Log($"重发次数超过10次, 关闭socket");
					OnDisconnect();
					return;
				}

				// 150
				if (TimeHelper.Now() - pack.time >= (pack.recurCount + 1) * overTime) {
					pack.recurCount += 1;
					uSocket.Send(pack.buffer, endPoint);
				}
			}

			CheckTimeOut();
		}

		private void OnDisconnect()
		{
			handleAction = null;
			uSocket.Close();
		}
	}
}

