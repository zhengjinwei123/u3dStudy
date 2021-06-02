using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class RoomListPanel : BasePanel {

	private RectTransform battleRes;
	private RectTransform roomList;
	private VerticalLayoutGroup roomLayout;
	private GameObject roomItemPrefab;

	private void Awake()
	{
		battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
		roomList = transform.Find("RoomList").GetComponent<RectTransform>();
		roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
		roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;

		transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
		transform.Find("RoomList/CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomButtonClick);
		transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshButtonClick);
	}

	private void Start() {

	}

	private void OnCreateRoomButtonClick()
	{
		PlayClickSound();
	}

	private void OnRefreshButtonClick()
	{
		PlayClickSound();
	}

	private void OnCloseButtonClick()
	{
		PlayClickSound();
		uiMgr.PopPanel();
	}

	public override void OnEnter()
	{
		base.OnEnter();

		EnterAnim();
	}

	public override void OnExit()
	{
		HideAnim();
	}

	public override void OnResume()
	{
		EnterAnim();
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


}
