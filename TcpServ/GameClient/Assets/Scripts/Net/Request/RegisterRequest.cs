using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RegisterRequest : BaseRequest
{
	private RegisterPanel registerPanel;
	public override void Awake()
	{
		reqCode = RequestCode.User;
		actionCode = ActionCode.Register;
		registerPanel = GetComponent<RegisterPanel>();

		base.Awake();
	}

	public void SendRequest(string username, string password)
	{
		string data = username + "," + password;
		base.SendRequest(data);
	}

	public override void OnResponse(string data)
	{
		ReturnCode returnCode = (ReturnCode)int.Parse(data);
		registerPanel.OnRegisterResponse(returnCode);
	}
}