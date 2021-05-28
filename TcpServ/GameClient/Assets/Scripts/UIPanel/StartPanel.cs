using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel {

	Button loginButton;
	private Animator btnAnimator;

	public override void OnEnter()
	{
		base.OnEnter();
		loginButton = transform.Find("LoginButton").GetComponent<Button>();
		btnAnimator = loginButton.GetComponent<Animator>();
		loginButton.onClick.AddListener(OnLoginButtonClick);
	}

	private void OnLoginButtonClick() {
		uiMgr.PushPanel(UIPanelType.Login);
	}

	public override void OnPause()
	{
		base.OnPause();
		loginButton.transform.DOScale(0, .5f).OnComplete(() =>
		{
			loginButton.gameObject .SetActive(false);
			btnAnimator.enabled = false;
		});
	}

	public override void OnResume()
	{
		base.OnResume();
	
		loginButton.gameObject.SetActive(true);

		loginButton.transform.DOScale(1, .5f);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
