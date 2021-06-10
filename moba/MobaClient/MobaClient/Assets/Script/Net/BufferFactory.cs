using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;


namespace Game.Net {
	public class BufferFactory
	{
		enum MessageType {
			ACK = 0,
			Logic = 1, // 业务报文
		}

		/// <summary>
		/// 创建并且发送报文
		/// </summary>
		/// <param name="messageID"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static BufferEntity CreateAndSendPackage(int messageID, IMessage message) {

			Debug.Log($"消息ID: {messageID} \n包体: {JsonHelper.SerializeObject(message)}");

			BufferEntity buffer = new BufferEntity(USocket.local.endPoint, USocket.local.sessionID, 0, 0, MessageType.Logic.GetHashCode(),
				messageID, ProtobufHelper.ToBytes(message));

			USocket.local.Send(buffer);
			return buffer;
		}
	}
}
