using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameRequest : BaseRequest {

	private RoomPanel roomPanel;

	public override void Awake()
	{
		reqCode = RequestCode.Game;
		actionCode = ActionCode.StartGame;
		roomPanel = GetComponent<RoomPanel>();
		base.Awake();
	}

	public override void SendRequest(string data)
	{
		base.SendRequest("r");
	}

	public override void OnResponse(string data)
	{
		ReturnCode returnCode = (ReturnCode)int.Parse(data);
		roomPanel.OnStartResponse(returnCode);
	}
}
