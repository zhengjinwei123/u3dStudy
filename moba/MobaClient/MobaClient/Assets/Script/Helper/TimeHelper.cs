using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeHelper  {
	//  1s = 1000ms 
	// 1毫秒 = 1000 微秒
	// 1微秒 = 1000纳秒
	// 一个计时周期表示 100纳秒， 就是一千万分之一秒， 1毫秒内有 10000 个计时周期， 就是 1秒内 有 1000万个计时周期
	private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
	/// <summary>
	/// 当前时间戳 毫秒级别
	/// </summary>
	/// <returns></returns>
	private static long ClientNow() {
		return (DateTime.UtcNow.Ticks - epoch) / 10000; // 得到毫秒级别
	}

	// 秒级别

	public static long ClientNowSeconds() {
		return (DateTime.UtcNow.Ticks - epoch) / 10000000; // 得到秒级别 
	}

	public static long Now() {
		return ClientNow();
	}
}
