using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Message
{
	public const int MAX_SIZE = 1024;

	private byte[] buffer = new byte[MAX_SIZE];
	private int startIndex = 0; // 存取了多少个字节的数据， 在数组里面

	public byte[] Data
	{
		get { return buffer; }
	}

	public int StartIndex
	{
		get { return startIndex; }
	}

	// 剩余空间
	public int RemainSize
	{
		get { return buffer.Length - startIndex; }
	}

	//public void AddCount(int count)
	//{
	//	startIndex += count;
	//}


	// 解析数据
	public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallback)
	{
		startIndex += newDataAmount;

		while (true)
		{
			if (startIndex <= 4) break;

			int count = BitConverter.ToInt32(buffer, 0);
			if ((startIndex - 4) >= count)
			{
				//string s = Encoding.UTF8.GetString(buffer, 4, count);
				//Console.WriteLine("读取到:" + s);
				ActionCode actionCode = (ActionCode)BitConverter.ToInt32(buffer, 4);
				//ActionCode actionCode = (ActionCode)BitConverter.ToInt32(buffer, 8);
				//string s = Encoding.UTF8.GetString(buffer, 12, count - 8);
				string s = Encoding.UTF8.GetString(buffer, 8, count - 4);


				processDataCallback(actionCode, s);

				Array.Copy(buffer, count + 4, buffer, 0, startIndex - 4 - count);
				startIndex -= (count + 4);
			}
			else
			{
				break;
			}
		}
	}

	public static byte[] PackData(RequestCode requestCode, string data)
	{

		byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
		byte[] dataBytes = Encoding.UTF8.GetBytes(data);
		int dataAmount = requestCodeBytes.Length + dataBytes.Length;
		byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

		return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>().Concat(dataBytes).ToArray<byte>();
	}

	public static byte[] PackData(RequestCode requestCode, ActionCode actionCode,  string data)
	{

		byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
		byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
		byte[] dataBytes = Encoding.UTF8.GetBytes(data);
		int dataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
		byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

		return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>().
			Concat(actionCodeBytes).ToArray<byte>().
			Concat(dataBytes).ToArray<byte>();
	}
}
