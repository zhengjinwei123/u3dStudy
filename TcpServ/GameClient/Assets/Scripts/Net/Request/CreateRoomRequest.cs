using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CreateRoomRequest : BaseRequest
{
	private RoomPanel roomPanel;

	public override void Awake()
	{
		reqCode = RequestCode.Room;
		actionCode = ActionCode.CreateRoom;
		base.Awake();
	}

	public void SetPanel(BasePanel panel) {
		roomPanel = panel as RoomPanel;
	}

	public override void SendRequest(string data)
	{
		base.SendRequest("r");
	}

	public override void OnResponse(string data)
	{
		//string[] strs = data.Split(',');
		ReturnCode returnCode = (ReturnCode)int.Parse(data);
		if (returnCode == ReturnCode.Success) {
			roomPanel.SetLocalPlayerResAsync();
		}
	}
}