using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanel : BasePanel {
	public override void OnEnter()
	{
		base.OnEnter();
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	public override void OnResume()
	{
		base.OnResume();
	}

	public void OnCloseButtonClick() {
		uiMgr.PopPanel();
	}
}
