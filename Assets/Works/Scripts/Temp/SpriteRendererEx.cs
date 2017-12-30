using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public static class SpriteRendererEx
{
	public class Delayable
	{
		SpriteRenderer _renderer;
		float _due;

		public Delayable(SpriteRenderer renderer,float due)
		{
			_renderer = renderer;
			_due = due;
		}

		public void SetAlpha(float a)
		{
			Observable.Timer(TimeSpan.FromSeconds(_due)).Subscribe(_ =>
			{
				_renderer.SetAlpha(a);
			});
		}
	}

	public static void SetAlpha(this SpriteRenderer self,float a)
	{
		var color = self.color;
		color.a = a;
		self.color = color;
	}

	public static Delayable Delay(this SpriteRenderer self,float due)
	{
		return new Delayable(self, due);
	}

	public static IDisposable Blink(this SpriteRenderer self,float interval)
	{
		return Observable.Interval(TimeSpan.FromSeconds(interval)).Subscribe(_ =>
		{
			if (self.color.a > 0.5f)
				self.SetAlpha(0);
			else
				self.SetAlpha(1);
		});
	}
}

public static class DisposableEx
{
	public class Delayable
	{
		IDisposable _target;
		float _due;

		public Delayable(IDisposable target,float due)
		{
			_target = target;
			_due = due;
		}

		public void Dispose()
		{
			Observable.Timer(TimeSpan.FromSeconds(_due)).Subscribe(_ =>
			{
				_target.Dispose();
			});
		}
	}

	public static Delayable Delay(this IDisposable self,float due)
	{
		return new Delayable(self, due);
	}
}