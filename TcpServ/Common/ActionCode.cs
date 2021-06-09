using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public enum ActionCode
	{
		None = 0,
		Login = 1,
		Register = 2,
		ListRoom = 3,
		CreateRoom = 4,
		JoinRoom = 5,
		UpdateRoom = 6,
		QuitRoom = 7,
		StartGame = 8,
		ShowTimer = 9,
		StartPlay = 10,
		Move = 11,
		Shoot = 12,
		Attack = 13,
		GameOver = 14,
		UpdateResult = 15,
		QuitBattle = 16
	}
}
