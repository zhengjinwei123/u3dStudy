using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ListRoomRequest :  BaseRequest
{
	private RoomListPanel roomListPanel;
	public override void Awake()
	{
		reqCode = RequestCode.Room;
		actionCode = ActionCode.ListRoom;
		roomListPanel = GetComponent<RoomListPanel>();
		base.Awake();
	}

	public override void SendRequest(string data)
	{
		base.SendRequest("r");
	}

	public override void OnResponse(string data)
	{
		List<UserData> udList = new List<UserData>();
		if (data != "0") {
			string[] udArray = data.Split('|');
			foreach (string ud in udArray) {
				string[] strs = ud.Split(',');
				udList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
			}
		}
		roomListPanel.LoadRoomItemAsync(udList);
	}
}
