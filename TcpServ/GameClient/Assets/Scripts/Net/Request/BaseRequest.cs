using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour {

	protected RequestCode reqCode = RequestCode.None;
	protected ActionCode actionCode = ActionCode.None;

	protected GameFacade _facade;

	protected GameFacade facade {
		get {
			if (_facade == null)
				_facade = GameFacade.Instance;
			return _facade;
		}
	}

	// Use this for initialization
	public virtual void Awake () {
		facade.AddRequest(actionCode, this);
	}

	public virtual void OnDestroy() {
		if (facade != null)
			facade.RemoveRequest(actionCode);
	}

	public virtual void SendRequest(string data) {
		facade.SendRequest(reqCode, actionCode, data);
	}

	public virtual void OnResponse(string data) {

	}
}
