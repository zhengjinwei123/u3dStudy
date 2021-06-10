using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Game.Net;

public class ProtoTest : MonoBehaviour
{
	USocket uSocket;

	void Start()
	{

		uSocket = new USocket(DispatchNetEvent);

		TestSend();


		//UserRegisterC2S userRegisterC2S1 = ProtobufHelper.FromBytes<UserRegisterC2S>(bufferEntity.proto);
	}

	private static void TestSend()
	{
		UserInfo userInfo = new UserInfo();
		userInfo.Account = "111";
		userInfo.Password = "123";

		UserRegisterC2S userRegisterC2S = new UserRegisterC2S();
		userRegisterC2S.UserInfo = userInfo;

		BufferEntity bufferEntity = BufferFactory.CreateAndSendPackage(1001, userInfo);
	}

	private void Update()
	{
		if (uSocket != null) {
			uSocket.Handle();
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			TestSend();
		}
	}

	void DispatchNetEvent(BufferEntity buffer) {

	}
}
