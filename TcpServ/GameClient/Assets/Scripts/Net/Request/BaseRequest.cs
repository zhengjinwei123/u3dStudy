using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour {

	protected RequestCode reqCode = RequestCode.None;
	protected ActionCode actionCode = ActionCode.None;

	// Use this for initialization
	public virtual void Awake () {
		GameFacade.Instance.AddRequest(actionCode, this);
	}

	public virtual void OnDestroy() {
		GameFacade.Instance.RemoveRequest(actionCode);
	}

	public virtual void SendRequest(string data) {
		GameFacade.Instance.SendRequest(reqCode, actionCode, data);
	}

	public virtual void OnResponse(string data) {

	}
}
