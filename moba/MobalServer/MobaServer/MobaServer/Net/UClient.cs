using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
	public class UClient
	{
		private USocket uSocket;
		public IPEndPoint endPoint;
		private int sendSN; // 发送的序号
		private int handleSN; // 处理的序号
		public int session;
		private Action<BufferEntity> dispatchNetEvent;

		public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int session, Action<BufferEntity> dispatchNetEvent)
		{
			this.uSocket = uSocket;
			this.endPoint = endPoint;
			this.sendSN = sendSN;
			this.handleSN = handleSN;
			this.session = session;
			this.dispatchNetEvent = dispatchNetEvent;

			// 超時檢測
			CheckOutTime();
		}

		public bool isConnected = true; // 是否处于连接的状态

		int overtime = 150; // ms
		// 对已发送的消息进行超时检测
		private async void CheckOutTime()
		{
			await Task.Delay(overtime);
			foreach (var package in sendPackage.Values)
			{
				if (package.recurCount >= 10) {
					Debug.LogError($"重发10次还是失败: 协议ID: {package.messageID}");
					uSocket.RemoveClient(session);
					return;
				}

				if (TimeHelper.Now() - package.time >= (package.recurCount + 1) * overtime) {
					package.recurCount += 1;
					Debug.Log($"超时重发: {package.sn}| {package.messageID}");
					uSocket.Send(package.buffer, endPoint);
				}
			}

			CheckOutTime();
		}

		public void Handle(BufferEntity buffer)
		{
			switch (buffer.messageType) {
				case 0: // ack 确认报文
					BufferEntity outBuffer;
					if (sendPackage.TryRemove(buffer.sn, out outBuffer))
					{
						Debug.Log($"报文已经确认, 序号: {buffer.sn}");
					}
					else {
						Debug.Log($"要确认的报文不存在, 序号: {buffer.sn}");
					}
					break;
				case 1: // 业务报文
					// 测试代码
					//if (buffer.sn != 1) {
					//	return;
					//}

					Debug.Log($"业务报文, 序号: {buffer.sn}");

					BufferEntity ackPackage = new BufferEntity(buffer);
					uSocket.SendACK(ackPackage, endPoint);

					// 在进行业务报文的处理
					HandleLogicPackage(buffer);
					break;
			}
		}

		// 存儲错序的报文， 等待后续处理
		ConcurrentDictionary<int, BufferEntity> awaitHandle = new ConcurrentDictionary<int, BufferEntity>();
		private void HandleLogicPackage(BufferEntity buffer)
		{
			if (buffer.sn <= handleSN) {
				Debug.Log($"已经处理过的消息, 序号: {buffer.sn}");
				return;
			}

			if (buffer.sn - handleSN > 1) {
				if (awaitHandle.TryAdd(buffer.sn, buffer)) {
					Debug.Log($"错序的报文进行缓存, 序号: {buffer.sn} | {handleSN}");
				}
				return;
			}

			handleSN = buffer.sn;
			if (dispatchNetEvent != null) {

				Debug.Log("分发消息");
				dispatchNetEvent(buffer);
			}
			BufferEntity nextBuffer;
			if (awaitHandle.TryRemove(handleSN + 1, out nextBuffer)) {
				HandleLogicPackage(nextBuffer);
			}
		}

		// 存储已经发送的报文， 用于超时重发
		ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>();
		// 发送的接口
		public void Send(BufferEntity package) {
			if (isConnected == false) {
				return;
			}
			package.time = TimeHelper.Now();
			sendSN += 1;

			package.sn = sendSN;

			package.Encoder(false);

			uSocket.Send(package.buffer, endPoint);

			if (session != 0) {
				// 缓存已经发送的数据
				sendPackage.TryAdd(package.sn, package);
			}

		}
		
		// 处理业务


		internal void Close()
		{
			isConnected = false;
		}
	}
}
