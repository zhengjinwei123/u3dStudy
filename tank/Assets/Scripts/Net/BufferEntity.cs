using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Game.Net {

	public enum MessageType {
		ACK = 0, // 确认报文
		LOGIC = 1, // 逻辑报文
	}

	public class BufferEntity
	{
		public int recurCount = 0;// 重发次数， 

		public IPEndPoint endPoint; // 发送的目标终端

		public int protoSize;
		public int sessionID; // 会话ID
		public int sn;// 序号
		public int moduleID;//模块ID
		public long time; // 发送时间
		public int messageType; // 协议类型
		public int messageID; // 协议ID
		public byte[] proto; // 业务报文

		public byte[] buffer;// 最终要发送的数据 或者是 收到的数据

		public BufferEntity(IPEndPoint endPoint, int sessionID, int sn, int moduleID, int messageType, int messageID, byte[] proto) {
			protoSize = proto.Length;

			this.endPoint = endPoint;
			this.sessionID = sessionID;
			this.sn = sn;
			this.moduleID = moduleID;
			this.time = 0;
			this.messageType = messageType;
			this.messageID = messageID;
			this.proto = proto;
		}

		// 编码
		public byte[] Encoder(bool isAck) {
			byte[] data = new byte[32 + protoSize];

			if (isAck) {
				protoSize = 0;
			}

			byte[] _length = BitConverter.GetBytes(protoSize);
			byte[] _sessionID = BitConverter.GetBytes(sessionID);
			byte[] _sn = BitConverter.GetBytes(sn);
			byte[] _moduleID = BitConverter.GetBytes(moduleID);
			byte[] _time = BitConverter.GetBytes(time);
			byte[] _messageType = BitConverter.GetBytes(messageType);
			byte[] _messageID = BitConverter.GetBytes(messageID);

			Array.Copy(_length, 0, data, 0, 4);
			Array.Copy(_sessionID, 0, data, 4, 4);
			Array.Copy(_sn, 0, data, 8, 4);
			Array.Copy(_moduleID, 0, data, 12, 4);
			Array.Copy(_time, 0, data, 16, 8);
			Array.Copy(_messageType, 0, data, 24, 4);
			Array.Copy(_messageID, 0, data, 28, 4);

			if (false == isAck) {
				Array.Copy(proto, 0, data, 32, proto.Length);
			}

			buffer = data;
			return data;
		}

		// 构建接收到的报文
		public BufferEntity(IPEndPoint endPoint, byte[] buffer) {
			this.endPoint = endPoint;
			this.buffer = buffer;

			DeCode();
		}

		// 反序列化报文
		public bool isFull = false;
		private void DeCode()
		{
			if (buffer.Length < 4) {
				isFull = false;
				return;
			}

			protoSize = BitConverter.ToInt32(buffer, 0);
			if (buffer.Length == protoSize + 32)
			{
				isFull = true;
			}
			else {
				isFull = false;
				return;
			}

			sessionID = BitConverter.ToInt32(buffer, 4);
			sn = BitConverter.ToInt32(buffer, 8);
			moduleID = BitConverter.ToInt32(buffer, 12);
			time = BitConverter.ToInt64(buffer, 16);
			messageType = BitConverter.ToInt32(buffer, 24);
			messageID = BitConverter.ToInt32(buffer, 28);

			// 确认报文
			if (messageType != 0) {
				proto = new byte[protoSize];
				Array.Copy(buffer, 32, proto, 0, protoSize);
			}
		}

		// 创建一个ACK报文
		public BufferEntity(BufferEntity package) {
			protoSize = 0;
			this.endPoint = package.endPoint;
			this.sessionID = package.sessionID;
			this.sn = package.sn;
			this.time = 0;
			this.moduleID = package.moduleID;
			this.messageType = 0;
			this.messageID = package.messageID;

			buffer = Encoder(true);
		}
	}
}

