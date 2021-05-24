using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Net;
using System;
using Cs;

public class GameManager : MonoBehaviour {

	public static USocket uSocket;
	// Use this for initialization
	void Start () {
		uSocket = new USocket(dispathNetEvent);
	}

	private void dispathNetEvent(BufferEntity buffer)
	{
		NetEvent.Instance.Dispatch(buffer.messageID, buffer);
	}

	// Update is called once per frame
	void Update () {
		if (uSocket != null) {
			uSocket.Handle();
		}
	}
}
