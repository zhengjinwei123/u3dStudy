﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : FSMState
{
	private void Awake()
	{
		stateID = StateID.Menu;
		AddTransition(Transition.StartButtonClick, StateID.Play);
	}

	public override void DoBeforeEntering()
	{
		ctrl.view.ShowMenu();
		ctrl.cameraManager.ZoomOut();
	}


	public override void DoBeforeLeaving()
	{
		ctrl.view.HideMenu();
	}

	public void OnStartButtonClick() {

		ctrl.audioManager.PlayCusor();
		fsm.PerformTransition(Transition.StartButtonClick);
	}
}
