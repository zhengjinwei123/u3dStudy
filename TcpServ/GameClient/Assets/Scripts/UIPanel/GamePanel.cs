using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class GamePanel : BasePanel {

	private Text timer;
	private int time = -1;
	private Button successBtn;
	private Button exitBtn;
	private Button failButton;

	private QuitBattleRequest quitBattleRequest;

	private void Awake()
	{
		timer = transform.Find("Timer").GetComponent<Text>();
		timer.gameObject.SetActive(false);

		successBtn = transform.Find("SuccessButton").GetComponent<Button>();
		successBtn.onClick.AddListener(OnResultClick);
		successBtn.gameObject.SetActive(false);

		failButton = transform.Find("FailButton").GetComponent<Button>();
		failButton.onClick.AddListener(OnResultClick);
		failButton.gameObject.SetActive(false);

		exitBtn = transform.Find("ExitButton").GetComponent<Button>();
		exitBtn.onClick.AddListener(OnExitClick);
		exitBtn.gameObject.SetActive(false);

		quitBattleRequest = GetComponent<QuitBattleRequest>();
	}

	private void OnResultClick()
	{
		uiMgr.PopPanel();
		uiMgr.PopPanel();
		facade.GameOver();
	}

	private void OnExitClick()
	{
		quitBattleRequest.SendRequest("");
	}

	public override void OnEnter()
	{
		gameObject.SetActive(true);
	}

	public override void OnExit()
	{
		successBtn.gameObject.SetActive(false);
		failButton.gameObject.SetActive(false);
		exitBtn.gameObject.SetActive(false);

		gameObject.SetActive(false);
	}

	private void Update() {
		if (time > -1) {
			ShowTime(time);
			time = -1;
		}
	}

	public void ShowTime(int time) {
		if (time == 3) {
			exitBtn.gameObject.SetActive(true);
		}

		timer.gameObject.SetActive(true);
		timer.text = time.ToString();
		timer.transform.localScale = Vector3.one;
		Color tmpColor = timer.color;
		tmpColor.a = 1;
		timer.color = tmpColor;
		timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
		timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
		facade.PlayNormalSound(AudioManager.Sound_Alert);
	}

	public void OnExitResponse()
	{
		OnResultClick();
	}

	public void ShowTimeAsync(int time) {
		this.time = time;
	}

	public void OnGameOverResponse(ReturnCode returnCode) {
		Button tmpBtn = null;
		switch (returnCode) {
			case ReturnCode.Success:
				tmpBtn = successBtn;
				break;
			case ReturnCode.Fail:
				tmpBtn = failButton;
				break;
		}
		if (tmpBtn == null) {
			return;
		}

		tmpBtn.gameObject.SetActive(true);
		tmpBtn.transform.localScale = Vector3.zero;
		tmpBtn.transform.DOScale(1, 0.5f);
	}
	
}
