using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
public class QuitRoomRequest : BaseRequest
{
	private RoomPanel roomPanel;

	public override void Awake()
	{
		reqCode = RequestCode.Room;
		actionCode = ActionCode.QuitRoom;
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
		if (returnCode == ReturnCode.Success)
		{
			roomPanel.OnExitResponse();
		}
	}

}