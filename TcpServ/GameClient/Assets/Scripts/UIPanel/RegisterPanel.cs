using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
using System;

public class RegisterPanel : BasePanel {

	private InputField usernameIF;
	private InputField passwordIF;
	private InputField repasswordIF;

	private RegisterRequest registerRequest;

	private void Awake()
	{
		usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
		passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
		repasswordIF = transform.Find("RepeatPasswordLabel/PasswordInput").GetComponent<InputField>();

		transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterButtonClick);
		transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
		registerRequest = GetComponent<RegisterRequest>();
	}

	public void OnLoginResponse(ReturnCode returnCode)
	{

	}

	public override void OnEnter()
	{
		gameObject.SetActive(true);

		transform.localScale = Vector3.zero;
		transform.DOScale(1, .5f);

		transform.localPosition = new Vector3(1000, 0, 0);
		transform.DOLocalMove(Vector3.zero, .5f).OnComplete(() =>
		{
			transform.DOShakePosition(100);
		});

	}

	public override void OnExit()
	{
		base.OnExit();

		gameObject.SetActive(false);
	}

	public void OnCloseButtonClick() {
		PlayClickSound();
		//Debug.Log("OnCloseButtonClick");

		transform.DOScale(0, .5f);
		Tweener tweener = transform.DOLocalMove(new Vector3(100, 0, 0), .5f);

		tweener.OnComplete(() =>
		{
			uiMgr.PopPanel();
		});
	}

	public void OnRegisterButtonClick() {
		PlayClickSound();
		if (string.IsNullOrEmpty(usernameIF.text))
		{
			uiMgr.ShowMessageASync("用戶名不能為空"); return;
		}
		if (string.IsNullOrEmpty(passwordIF.text))
		{
			uiMgr.ShowMessageASync("密码不能为空"); return;
		}
		if (false == string.Equals(passwordIF.text, repasswordIF.text)) {
			uiMgr.ShowMessageASync("密码不一致"); return;
		}

		registerRequest.SendRequest(usernameIF.text, passwordIF.text);

	}

	public void OnRegisterResponse(ReturnCode returnCode)
	{
		if (returnCode == ReturnCode.Success)
		{
			// TODO
			uiMgr.ShowMessageASync("success");
		}
		else
		{
			uiMgr.ShowMessageASync("已存在");
		}
	}
}
