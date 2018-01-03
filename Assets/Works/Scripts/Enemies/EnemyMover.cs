using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

namespace Assets.Enemies
{
	public class EnemyMover : BaseEnemyComponent
	{
		//! ----parameters----
		[SerializeField]
		float _moveTick = 0.5f;
		[SerializeField]
		Vector2 _moveStep = new Vector2(0.2f, 0.2f);

		//! ----internal variables----
		BoxCollider2D _collider;
		bool _isRight = true;
		IDisposable _disposer;

		//! ----functions----
		void Move()
		{
			float step = _isRight ? _moveStep.x : -_moveStep.x;
			Vector2 center = _collider.transform.position.ToVector2() + _collider.offset;
			Vector2 size = _collider.bounds.size;

			var hit = Physics2D.BoxCast(center, size, 0f, Vector2.right, step, LayerMask.GetMask("Wall"));
			if (!hit)
				transform.AddPositionX(step);
			else
			{
				transform.AddPositionY(_moveStep.y);
				_isRight = !_isRight;
			}
		}

		//! ----overrides----
		public override void Initialize()
		{
			base.Initialize();
			core.onInitialized += () =>
			{
				_collider = GetComponentInChildren<BoxCollider2D>();
				_disposer = Observable.Interval(TimeSpan.FromSeconds(_moveTick)).Subscribe(_ =>
				{
					Move();
				}).AddTo(gameObject);
			};
			core.onReleased += () =>
			{
				_disposer.Dispose();
			};
		}
	}
}