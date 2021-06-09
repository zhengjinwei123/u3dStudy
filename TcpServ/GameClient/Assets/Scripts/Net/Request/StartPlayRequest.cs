using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StartPlayRequest :  BaseRequest
{
	public bool isStartPlaying = false;

	public override void Awake()
	{
		actionCode = ActionCode.StartPlay;
		base.Awake();
	}

	private void Update()
	{
		if (isStartPlaying) {
			facade.StartPlaying();
			isStartPlaying = false;
		}
	}

	public override void OnResponse(string data)
	{
		isStartPlaying = true;
	}
}
