using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class EventBase<T, P, X> where T: new() where P: class {

	private static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = new T();
			}
			return instance;
		}
	}

	// 存储事件id, 还有方法(委托)
	public ConcurrentDictionary<X, List<Action<P>>> eventDic = new ConcurrentDictionary<X, List<Action<P>>>();

	// 添加事件
	public void AddEventListener(X key, Action<P> handleFunc) {
		if (eventDic.ContainsKey(key))
		{
			eventDic[key].Add(handleFunc);
		}
		else {
			List<Action<P>> actions = new List<Action<P>>();
			actions.Add(handleFunc);
			eventDic[key] = actions;
		}
	}

	// 移除
	public void RemoveEventListener(X key, Action<P> handleFunc) {
		if (eventDic.ContainsKey(key)) {
			List<Action<P>> actions = eventDic[key];
			actions.Remove(handleFunc);

			if (actions.Count == 0) {
				List<Action<P>> removedActions;
				eventDic.TryRemove(key, out removedActions);
			}
		}
	}

	// 派发
	public void Dispatch(X key, P p) {
		if (false == eventDic.ContainsKey(key)) {
			return;
		}

		List<Action<P>> actions = eventDic[key];
		if (actions != null && actions.Count > 0) {
			for (int i = 0; i < actions.Count; ++i) {
				if (actions[i] != null) {
					actions[i](p);
				}
			}
		}
	}

	// 无参数 派发
	public void Dispatch(X key) {
		Dispatch(key, null);
	}
	
}
