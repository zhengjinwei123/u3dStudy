using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class LoginPanel : BasePanel {

	private Button closeButton;
	private InputField usernameIF;
	private InputField passwordIF;
	private LoginRequest loginRequest;


	public override void OnEnter()
	{
		base.OnEnter();
		gameObject.SetActive(true);
		

		transform.localScale = Vector3.zero;
		transform.DOScale(1, .5f);

		transform.localPosition = new Vector3(1000, 0, 0);
		transform.DOLocalMove(Vector3.zero, .5f).OnComplete(() => {
			transform.DOShakePosition(100);
		});

		closeButton = transform.Find("CloseButton").GetComponent<Button>();
		closeButton.onClick.AddListener(OnCloseButtonClick);

		usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
		passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();


		transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginButtonClick);
		transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterButtonClick);


		loginRequest = GetComponent<LoginRequest>();
	}

	private void OnLoginButtonClick()
	{
		string msg = "";
		if (string.IsNullOrEmpty(usernameIF.text)) {
			msg += "用戶名不能为空 ";
		}
		if (string.IsNullOrEmpty(passwordIF.text)) {
			msg += "密码不能为空 ";
		}

		if (msg != "") {
			uiMgr.ShowMessage(msg);return;
		}
		loginRequest.SendRequest(usernameIF.text, passwordIF.text);
	}

	private void OnRegisterButtonClick()
	{
		uiMgr.PushPanel(UIPanelType.Register);
	}

	private void OnCloseButtonClick()
	{
		transform.DOScale(0, .5f);
		Tweener tweener = transform.DOLocalMove(new Vector3(100, 0, 0), .5f);


		tweener.OnComplete(() => {
			uiMgr.PopPanel();
		});
	}

	public override void OnExit()
	{
		base.OnExit();

		gameObject.SetActive(false);
	}

	public override void OnResume()
	{
		base.OnResume();
	}
}
