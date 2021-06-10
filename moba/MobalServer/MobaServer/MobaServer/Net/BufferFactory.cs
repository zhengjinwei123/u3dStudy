using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaServer.Net
{
	class BufferFactory
	{
		enum MessageType
		{
			ACK = 0,
			Logic = 1, // 业务报文
		}

		/// <summary>
		/// 创建并且发送报文
		/// </summary>
		/// <param name="messageID"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static BufferEntity CreateAndSendPackage(UClient uClient, int messageID, IMessage message)
		{
			if (uClient.isConnected == false) {
				return null;
			}

			Debug.Log(messageID, message);

			BufferEntity bufferEntity = new BufferEntity(uClient.endPoint, uClient.session, 0, 0, MessageType.Logic.GetHashCode(),
				messageID, ProtobufHelper.ToBytes(message));
			uClient.Send(bufferEntity);
			return bufferEntity;
		}
	}
}
