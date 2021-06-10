using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace MobaServer.Net {
	public class BufferEntity
	{
		public int recurCount = 0;// 重发次数， 内部使用， 不是业务数据
		public IPEndPoint endPoint; // 发送的目标终端

		public int protoSize; // 报文大小
		public int session; // 会话ID
		public int sn; // 序号
		public int moduleID; // 模块ID
		public long time; // 发送时间
		public int messageType; // 协议类型
		public int messageID; // 协议ID
		public byte[] proto; // 业务报文

		public byte[] buffer; // 最终要发送的数据， 或者收到的数据

		// 构建请求报文
		public BufferEntity(IPEndPoint endPoint, int session, int sn, int moduleID, int messageType, int messageID, byte[] proto) {
			protoSize = proto.Length; // 业务报文数据的大小
			this.endPoint = endPoint;
			this.session = session;
			this.sn = sn;
			this.moduleID = moduleID;
			this.messageType = messageType;
			this.messageID = messageID;
			this.proto = proto;
		}

		// 编码的接口  Ack 确认报文 ， 业务报文
		public byte[] Encoder(bool isAck) {
			byte[] data = new byte[32 + protoSize];

			if (isAck == true) {
				protoSize = 0; // 发送的业务数据的大小
			}

			byte[]  _length = BitConverter.GetBytes(protoSize);
			byte[] _session = BitConverter.GetBytes(session);
			byte[] _sn = BitConverter.GetBytes(sn);
			byte[] _moduleID = BitConverter.GetBytes(moduleID);
			byte[] _time = BitConverter.GetBytes(time);
			byte[] _messageType = BitConverter.GetBytes(messageType);
			byte[] _messageID = BitConverter.GetBytes(messageID);

			// 要将字节数组写入到data
			Array.Copy(_length, 0, data, 0, 4);
			Array.Copy(_session, 0, data, 4, 4);
			Array.Copy(_sn, 0, data, 8, 4);
			Array.Copy(_moduleID, 0, data, 12, 4);
			Array.Copy(_time, 0, data, 16, 8);
			Array.Copy(_messageType, 0, data, 24, 4);
			Array.Copy(_messageID, 0, data, 28, 4);

			if (isAck == false) {
				Array.Copy(proto, 0, data, 32, proto.Length);
			}
			buffer = data;
			return data;
		}

		/// <summary>
		/// 构建接收到的报文实体
		/// </summary>
		/// <param name="endPoint">终端IP</param>
		/// <param name="buffer">收到的数据</param>
		public BufferEntity(IPEndPoint endPoint, byte[] buffer) {
			this.endPoint = endPoint;
			this.buffer = buffer;
			DeCode();
		}

		public bool isFull = false;
		// 将报文反序列化 成员
		private void DeCode() {
			if (buffer.Length >= 4)
			{
				protoSize = BitConverter.ToInt32(buffer, 0); // 从 0 的位置取4个字节
				if (buffer.Length == protoSize + 32)
				{

					isFull = true;
				}
				else {
					isFull = false;
				}
			}
			else {
				return;
			}

			if (isFull == false) {
				return;
			}
			// 将字节数组转化成 int 或者 long

			session = BitConverter.ToInt32(buffer, 4);// 从 4 的位置取4个字节
			sn = BitConverter.ToInt32(buffer, 8); // 从 8 的位置取4个字节
			moduleID = BitConverter.ToInt32(buffer, 12);// 从 12 的位置取4个字节
			time = BitConverter.ToInt64(buffer, 16);// 从 20 的位置取8个字节
			messageType = BitConverter.ToInt32(buffer, 24);// 从 24 的位置取4个字节
			messageID = BitConverter.ToInt32(buffer, 28); // 从 28 的位置取4个字节

			if (messageType == 0)
			{
				// ack 报文
			}
			else
			{
				proto = new byte[protoSize];
				// 将buffer 里剩下的数据复制到proto (得到最终的业务数据)
				Array.Copy(buffer, 32, proto, 0, protoSize);
			}
		}


		/// <summary>
		/// 创建一个Ack报文实体
		/// </summary>
		/// <param name="package">收到的报文实体, 已经反序列化好的</param>
		public BufferEntity(BufferEntity package) {
			protoSize = 0;
			this.endPoint = package.endPoint;
			this.session = package.session;
			this.sn = package.sn;
			this.time = 0;
			this.messageType = 0;
			this.messageID = package.messageID;
			buffer = Encoder(true);
		}
	}
}

