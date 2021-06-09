using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager {

	private UserData userData;

	private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();

	private Transform rolePositions;
	private RoleType currentRoleType;
	private GameObject currentRoleGameObejct;
	

	public PlayerManager(GameFacade facade) : base(facade)
	{

	}

	public void SetCurrentRoleType(RoleType rt) {
		currentRoleType = rt;
	}

	public UserData UserData {
		set { userData = value; }
		get { return userData; }
	}

	private void InitRoleDict() {
		roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Arrow_BLUE"));
		roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Arrow_RED"));
	}
}
