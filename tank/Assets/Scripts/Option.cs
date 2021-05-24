using Game.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cs;

public class Option : MonoBehaviour {

	private int choice = 1;

	public Transform posOne;
	public Transform posTwo;
	// Use this for initialization

	private void Awake()
	{
		NetEvent.Instance.AddEventListener(Cs.ID.S2CUserLogin.GetHashCode(), OnC2SUserLogin);
	}

	private void OnC2SUserLogin(BufferEntity buffer) {
		S2C_UserLogin response = ProtobufHelper.FromBytes<S2C_UserLogin>(buffer.proto);
		Debug.Log($"登录返回 OnC2SUserLogin: {response.Result}");
	}

	void Start () {
		
	}

	private void loginRequest() {
		C2S_UserLogin req = new C2S_UserLogin();
		req.UserInfo = new UserInfo();
		req.UserInfo.Account = "zjw";
		req.UserInfo.Password = "123456";

		BufferFactory.CreateAndSendMessage(Cs.ID.C2SUserLogin.GetHashCode(), req);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W))
		{
			choice = 1;
			transform.position = posOne.position;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			choice = 2;
			transform.position = posTwo.position;
		}
		if (choice == 1 && Input.GetKeyDown(KeyCode.Space))
		{
			loginRequest();
			//SceneManager.LoadScene(1);
		}
	}
}
