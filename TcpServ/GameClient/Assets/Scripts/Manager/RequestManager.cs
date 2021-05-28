using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : BaseManager {
	private Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();

	public RequestManager(GameFacade facade) : base(facade)
	{

	}

	public void AddRequest(ActionCode actionCode, BaseRequest request) {
		requestDic.Add(actionCode, request);
	}

	public void RemoveRequest(ActionCode actionCode) {
		requestDic.Remove(actionCode);
	}

	public void HandleResponse(ActionCode actionCode, string data)
	{
		BaseRequest req = requestDic.TryGet(actionCode);
		if (req == null)
		{
			Debug.Log("无法得到actionCode [" + actionCode + " ] 对应的request类");
			return;
		}
		req.OnResponse(data);
	}


}
