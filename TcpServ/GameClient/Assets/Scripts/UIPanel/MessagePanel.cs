using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel {

	private Text text;
	private float showTime = 1;

	public override void OnEnter()
	{
		base.OnEnter();
		text = GetComponent<Text>();
		text.enabled = false;

		uiMgr.InjectMsgPanel(this);
	}

	public void ShowMessage(string msg) {

		text.CrossFadeAlpha(1, .2f, false);
		text.text = msg;
		text.enabled = true;

		Invoke("Hide", showTime);
	}

	private void Hide() {
		text.CrossFadeAlpha(0, .5f, false);
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	public override void OnResume()
	{
		base.OnResume();
	}
}
