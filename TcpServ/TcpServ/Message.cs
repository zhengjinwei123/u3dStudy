using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServ
{
	class Message
	{
		public const int MAX_SIZE = 1024;

		private byte[] buffer = new byte[MAX_SIZE];
		private int startIndex = 0; // 存取了多少个字节的数据， 在数组里面

		public byte[] Data {
			get { return buffer;  }
		}

		public int StartIndex {
			get { return startIndex;  }
		}

		// 剩余空间
		public int RemainSize {
			get { return buffer.Length - startIndex;  }
		}

		public void AddCount(int count) {
			startIndex += count;
		}


		// 解析数据
		public void ReadMessage() {
			while (true) {
				if (startIndex <= 4) break;

				int count = BitConverter.ToInt32(buffer, 0);
				if ((startIndex - 4) >= count)
				{
					string s = Encoding.UTF8.GetString(buffer, 4, count);
					Console.WriteLine("读取到:" + s);
					Array.Copy(buffer, count + 4, buffer, 0, startIndex - 4 - count);
					startIndex -= (count + 4);
				}
				else {
					break;
				}
			}
		}

	}
}
