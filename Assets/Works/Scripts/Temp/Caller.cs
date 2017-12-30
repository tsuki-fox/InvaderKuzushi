using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class Caller
{
	public delegate void Handler();

	public static void Timer(float dueSeconds, Handler handler, GameObject addFrom = null)
	{
		if (!addFrom)
			Observable.Timer(TimeSpan.FromSeconds(dueSeconds)).Subscribe(_ => handler());
		else
			Observable.Timer(TimeSpan.FromSeconds(dueSeconds)).Subscribe(_ => handler()).AddTo(addFrom);
	}
}