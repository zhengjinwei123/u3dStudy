using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;

public class ClientManager: BaseManager  {
	private const string IP = "127.0.0.1";
	private const int PORT = 18001;
	private Socket clientSocket;
	private Message msg;

	public ClientManager(GameFacade facade) : base(facade) { }

	public override void OnInit() {
		base.OnInit();
		msg = new Message();
		clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		try
		{
			clientSocket.Connect(IP, PORT);
			Start();
		}
		catch (Exception e) {
			Debug.LogWarning("无法连接到服务器， 请检查网络:" + e);
		}
	}

	private void Start() {
		clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
	}

	private void ReceiveCallback(IAsyncResult ar)
	{
		try
		{
			int count = clientSocket.EndReceive(ar);

			msg.ReadMessage(count, OnProcessData);
			Start();
		}
		catch (Exception e) {
			Debug.Log(e);
		}

	}

	private void OnProcessData(ActionCode actionCode, string data)
	{
		// TODO
		gameFacade.HandleResponse(actionCode, data);
	}

	public void SendRequest(RequestCode reqCode, ActionCode actionCode, string data) {
		byte[] bytes = Message.PackData(reqCode, actionCode, data);
		clientSocket.Send(bytes);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();

		try
		{
			if (clientSocket != null) {
				clientSocket.Close();
			}
		}
		catch (Exception e) {
			Debug.LogWarning("无法关闭连接:" + e);
		}
	}
}
