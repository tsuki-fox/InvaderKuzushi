using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class Caller
{
	public delegate void Handler();

	public static IDisposable Timer(float dueSeconds, Handler handler, GameObject addFrom = null)
	{
		if (!addFrom)
			return Observable.Timer(TimeSpan.FromSeconds(dueSeconds)).Subscribe(_ => handler());
		else
			return Observable.Timer(TimeSpan.FromSeconds(dueSeconds)).Subscribe(_ => handler()).AddTo(addFrom);
	}

	public static IObservable<long> Timer(float due)
	{
		return Observable.Timer(TimeSpan.FromSeconds(due));
	}
	public static IObservable<long> Interval(float due)
	{
		return Observable.Interval(TimeSpan.FromSeconds(due));
	}
}