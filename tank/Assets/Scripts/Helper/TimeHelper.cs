using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeHelper  {
	private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

	// 毫秒 时间戳
	private static long ClientNow() {
		return (DateTime.UtcNow.Ticks - epoch) / 1000;
	}

	public static long NowSeconds() {
		return (DateTime.UtcNow.Ticks - epoch) / 1000000;
	}

	public static long Now() {
		return ClientNow();
	}

}
