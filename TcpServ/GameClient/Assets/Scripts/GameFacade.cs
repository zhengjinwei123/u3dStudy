using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour {

	private UIManager uiMgr;
	private AudioManager audioMgr;
	private PlayerManager playerMgr;
	private CameraManager cameraMgr;
	private RequestManager requestMgr;
	private ClientManager clientMgr;

	public void StartPlaying()
	{

	}

	private bool isEnterPlaying = false;

	private static GameFacade _instance;
	public static GameFacade Instance {
		get { return _instance;  }
	}

	private void Awake()
	{
		if (_instance != null) {
			Destroy(this.gameObject); return;
		}
		_instance = this;
	}

	

	public void AddRequest(ActionCode actionCode, BaseRequest request)
	{
		requestMgr.AddRequest(actionCode, request);
	}

	public void RemoveRequest(ActionCode actionCode)
	{
		requestMgr.RemoveRequest(actionCode);
	}

	public void HandleResponse(ActionCode actionCode, string data)
	{
		requestMgr.HandleResponse(actionCode, data);

	}

	private void InitManager() {
		uiMgr = new UIManager(this);
		audioMgr = new AudioManager(this);
		playerMgr = new PlayerManager(this);
		cameraMgr = new CameraManager(this);
		requestMgr = new RequestManager(this);
		clientMgr = new ClientManager(this);

		uiMgr.OnInit();
		audioMgr.OnInit();
		playerMgr.OnInit();
		cameraMgr.OnInit();
		requestMgr.OnInit();
		clientMgr.OnInit();
	}

	public void GameOver()
	{

	}

	private void DestroyManager() {
		uiMgr.OnDestroy();
		audioMgr.OnDestroy();
		playerMgr.OnDestroy();
		cameraMgr.OnDestroy();
		requestMgr.OnDestroy();
		clientMgr.OnDestroy();
	}

	// Use this for initialization
	void Start () {
		InitManager();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateManager();
		if (isEnterPlaying) {
			EnterPlaying();
			isEnterPlaying = false;
		}
	}

	private void EnterPlaying() {

	}

	private void UpdateManager() {
		uiMgr.Update();
		audioMgr.Update();
		playerMgr.Update();
		cameraMgr.Update();
		requestMgr.Update();
		clientMgr.Update();
	}

	private void OnDestroy()
	{
		DestroyManager();
	}


	public void ShowMessage(string msg) {
		uiMgr.ShowMessage(msg);
	}


	public void SendRequest(RequestCode reqCode, ActionCode actionCode, string data)
	{
		clientMgr.SendRequest(reqCode, actionCode, data);
	}

	public void PlayBgSound(string soundName) {
		audioMgr.PlayBgSound(soundName);
	}

	public void PlayNormalSound(string soundName) {
		audioMgr.PlayNormalSound(soundName);
	}

	public void SetUserData(UserData ud) {
		playerMgr.UserData = ud;
	}

	public UserData GetUserData() {
		return playerMgr.UserData;
	}

	public void EnterPlayingAsync()
	{
		isEnterPlaying = true;
		Debug.Log("进入游戏了");
	}
}
