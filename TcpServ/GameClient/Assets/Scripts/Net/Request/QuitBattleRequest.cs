using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class QuitBattleRequest : BaseRequest
{
	private bool isQuitBattle = false;
	private GamePanel gamePanel;
	public override void Awake()
	{
		reqCode = RequestCode.Game;
		actionCode = ActionCode.QuitBattle;
		gamePanel = GetComponent<GamePanel>();
		base.Awake();
	}

	private void Update()
	{
		if (isQuitBattle) {
			gamePanel.OnExitResponse();
			isQuitBattle = false;
		}
	}

	public override void OnResponse(string data)
	{
		isQuitBattle = true;
	}

	public override void SendRequest(string data)
	{
		base.SendRequest("r");
	}
}
