using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net {

	// 客户端代理
	public class UClient
	{
		public IPEndPoint endPoint;
		USocket uSocket;
		public int sessionID; // 会话ID
		public int sendSN = 0;// 发送序号
		public int handleSN = 0; // 处理的序号， 为了保证报文的顺序性

		Action<BufferEntity> handleAction; // 处理报文的函数, 实际就是分发报文给各个模块

		public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionID, Action<BufferEntity> dispatchNetEvent) {
			this.uSocket = uSocket;
			this.endPoint = endPoint;
			this.sendSN = sendSN;
			this.handleSN = handleSN;
			this.sessionID = sessionID;
			handleAction = dispatchNetEvent;

			CheckOutTime();
		}

		// 处理消息： 按照报文的序号进行顺序处理， 如果是收到超过当前顺序 + 1 的报文， 先进行缓存
		public void Handle(BufferEntity buffer) {
			if (this.sessionID == 0 && buffer.session != 0)
			{
				Debug.Log($"服务器发送给我们的会话ID是: {buffer.session}");
				this.sessionID = buffer.session;
			}
			else {
				if (buffer.session != this.sessionID) {
					Debug.Log($"非法请求");
				}
			}

			switch (buffer.messageType) {
				case 0: // Ack 确认报文
					BufferEntity outBufferEntity;
					if (sendPackage.TryRemove(buffer.sn, out outBufferEntity)) {
						Debug.Log($"收到ACK 确认报文, 序号是:{buffer.sn}");
					}
					break;
				case 1: // 业务报文
					BufferEntity ackPackage = new BufferEntity(buffer);
					uSocket.SendACK(ackPackage);// 先告诉服务器，我已经收到报文了

					// 处理业务报文
					HandleLogicPackage(buffer);
					break;
			}
		}

		ConcurrentDictionary<int, BufferEntity> awaitHandle = new ConcurrentDictionary<int, BufferEntity>();
		// 处理业务报文
		void HandleLogicPackage(BufferEntity buffer) {


			if (buffer.sn <= handleSN) {
				return;
			}

			if (buffer.sn - handleSN > 1) {
				if (awaitHandle.TryAdd(buffer.sn, buffer)) {
					Debug.Log($"收到錯序的报文: {buffer.sn}");
				}
				return;
			}

			// 更新一下已经处理的报文
			handleSN = buffer.sn;

			if (handleAction != null) {

				// 分发给游戏模块处理
				handleAction(buffer);
			}

			// 判断缓存区有没有下一条可以处理的数据
			BufferEntity nextBuffer;
			if (awaitHandle.TryRemove(handleSN + 1, out nextBuffer)) {
				HandleLogicPackage(nextBuffer);
			}

		}



		// 缓存已经发送的报文
		ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>();
		// 发送报文的接口
		public void Send(BufferEntity package) {
			package.time = TimeHelper.Now();
			sendSN += 1;
			package.sn = sendSN;
			package.Encoder(false);
			if (sessionID != 0)
			{
				// 缓存起来， 因为可能需要重发 (超时重传)
				sendPackage.TryAdd(sendSN, package);
			}
			else {
				// 还没和服务器建立连接， 所以不需要进行缓存
				Debug.Log("还没和服务器建立连接");
			}
			uSocket.Send(package.buffer, endPoint);
		}

		int overtime = 150;// ms
		// 超时重传检测的接口
		public async void CheckOutTime() {
			await Task.Delay(overtime);
			foreach (var package in sendPackage.Values)
			{
				// 确定是不是超过最大发送次数， 关闭socket
				if (package.recurCount >= 10) {
					Debug.LogError($"重发10次还是失败: 协议ID: {package.messageID}");
					OnDisconnect();
					return;
				}

				if (TimeHelper.Now() - package.time >= (package.recurCount + 1) * overtime) {
					package.recurCount += 1;
					// 超时重传
					Debug.Log($"超时重发,  {package.messageID}| {package.recurCount}");

					uSocket.Send(package.buffer, endPoint);
				}
			}

			CheckOutTime();
		}

		public void OnDisconnect() {
			handleAction = null;
			uSocket.Close();
		}

	}
}

