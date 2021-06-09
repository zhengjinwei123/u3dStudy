using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Common;

public class RoomListPanel : BasePanel {

	private RectTransform battleRes;
	private RectTransform roomList;
	private VerticalLayoutGroup roomLayout;
	private GameObject roomItemPrefab;

	private CreateRoomRequest createRoomRequest;
	private ListRoomRequest listRoomRequest;
	private JoinRoomRequest joinRoomRequest;

	private List<UserData> udList = null;
	private UserData ud1 = null;
	private UserData ud2 = null;

	private void Awake()
	{
		battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
		roomList = transform.Find("RoomList").GetComponent<RectTransform>();
		roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
		roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;

		transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
		transform.Find("RoomList/CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomButtonClick);
		transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshButtonClick);

		createRoomRequest = GetComponent<CreateRoomRequest>();
		listRoomRequest = GetComponent<ListRoomRequest>();
		joinRoomRequest = GetComponent<JoinRoomRequest>();
	}

	public void LoadRoomItemAsync(List<UserData> udList)
	{
		this.udList = udList;
	}

	private void Start() {

	}

	private void OnCreateRoomButtonClick()
	{
		PlayClickSound();
		BasePanel panel = uiMgr.PushPanel(UIPanelType.Room);
		createRoomRequest.SetPanel(panel);
		createRoomRequest.SendRequest("r");
	}

	private void OnRefreshButtonClick()
	{
		PlayClickSound();
		listRoomRequest.SendRequest("r");
	}

	private void OnCloseButtonClick()
	{
		PlayClickSound();
		uiMgr.PopPanel();
	}

	public override void OnEnter()
	{
		SetBattleRes();

		EnterAnim();
		listRoomRequest.SendRequest("r");
	}

	public override void OnExit()
	{
		HideAnim();
	}

	public override void OnPause()
	{
		HideAnim();
	}

	public override void OnResume()
	{
		EnterAnim();
		listRoomRequest.SendRequest("r");
	}
	private void EnterAnim() {
		gameObject.SetActive(true);

		battleRes.localPosition = new Vector3(-1000, 0);
		battleRes.DOLocalMoveX(-382, 0.5f);

		roomList.localPosition = new Vector3(1000, 0);
		roomList.DOLocalMoveX(154, 0.5f);
	}

	private void HideAnim() {
		battleRes.DOLocalMoveX(-1000, 0.5f);

		roomList.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false) );
	}

	private void SetBattleRes() {
		UserData ud = facade.GetUserData();
		if (ud == null) {
			return;
		}

		transform.Find("BattleRes/Username").GetComponent<Text>().text = ud.Username;
		transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数:" + ud.TotalCount.ToString();
		transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利:" + ud.WinCount.ToString();
	}

	//private void LoadRoomItem(int count) {
	//	for (int i = 0; i < count; ++i) {
	//		GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
	//		roomItem.transform.SetParent(roomLayout.transform);
	//	}
	//}

	private void Update()
	{
		if (udList != null) {
			LoadRoomItem(udList);
			udList = null;
		}

		if (ud1 != null && ud2 != null) {
			BasePanel panel = uiMgr.PushPanel(UIPanelType.Room);
			(panel as RoomPanel).SetAllPlayerResAsync(ud1, ud2);
			ud1 = null;
			ud2 = null;
		}
	}

	private void LoadRoomItem(List<UserData> udList) {
		RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
		foreach (RoomItem ri in riArray) {
			ri.DestroySelf();
		}

		int count = udList.Count;
		for (int i = 0; i < count; ++i) {
			GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
			roomItem.transform.SetParent(roomLayout.transform);
			UserData ud = udList[i];
			roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id, ud.Username, ud.TotalCount, ud.WinCount, this);
		}
	}

	public void OnJoinClick(int roomId) {
		joinRoomRequest.SendRequest(roomId);
	}

	public void OnJoinResponse(ReturnCode returnCode, UserData ud1, UserData ud2) {
		switch (returnCode) {
			case ReturnCode.NotFound:
				uiMgr.ShowMessageASync("房间不存在");
				break;
			case ReturnCode.Fail:
				uiMgr.ShowMessageASync("房间满了");
				break;
			case ReturnCode.Success:
				this.ud1 = ud1;
				this.ud2 = ud2;
				break;
		}
	}
}
