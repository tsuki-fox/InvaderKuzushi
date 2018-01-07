using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;
using Assets.Balls;
using UniRx;
using System;

namespace Assets.Managers
{
	public class BallManager : MonoBehaviour
	{
		//! ----parameters----
		[SerializeField]
		GameObject _ballSource;
		[SerializeField]
		Vector2 _spawnPos;
		[SerializeField]
		GameObject _spawnVFX;
		[SerializeField]
		AudioClip _spawnSE;
		[SerializeField]
		float _spawnDelay = 2f;

		//! ----internal----
		GameObject _ball;
		IDisposable _disposer;

		//! ----functions----
		GameObject SpawnBall()
		{
			var ball = ObjectPool.Alloc(_ballSource);
			ball.transform.position = _spawnPos;
			ObjectPool.Alloc(_spawnVFX, 1f, _spawnPos);
			GlobalAudioSource.PlayOneShot(_spawnSE);

			var ballCore = ball.GetComponent<BallCore>();
			ballCore.onKilled += () =>
			{
				_disposer = Observable.Timer(System.TimeSpan.FromSeconds(_spawnDelay)).Subscribe(_ =>
				  {
					  _ball = SpawnBall();
				  }).AddTo(ballCore.gameObject);
			};

			ball.GetComponent<Rigidbody2D>().AddForce(Vector2.up);

			return ball;
		}

		void Start()
		{
			SceneConductor.onPlaySceneStart += () =>
			{
				Caller.Timer(1f).Subscribe(_ =>
				{
					_ball = SpawnBall();
				});
			};
			SceneConductor.onPlaySceneEnd += () =>
			{
				if(_disposer!=null)
					_disposer.Dispose();
				ObjectPool.Free(_ball);
			};
		}
	}
}