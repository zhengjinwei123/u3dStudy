using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net {
	public class BufferFactory
	{
		public static BufferEntity CreateAndSendMessage(int messageID, IMessage message)
		{
			BufferEntity buffer = new BufferEntity(USocket.local.endPoint, USocket.local.sessionID, 0, 0, MessageType.LOGIC.GetHashCode(),
				messageID, ProtobufHelper.ToBytes(message));
			USocket.local.SendMessage(buffer);
			return buffer;
		}
	}

}
