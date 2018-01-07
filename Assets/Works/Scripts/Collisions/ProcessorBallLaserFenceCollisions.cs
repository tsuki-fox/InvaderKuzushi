using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorBallLaserFenceCollisions : BaseProcessor
	{
		[Header("ボール-レーザーフェンス 衝突処理")]
		//! ----parameters----
		[SerializeField]
		GameObject _vfx;
		[SerializeField]
		AudioClip _se;

		//! ----functions----
		void Awake()
		{
			CollisionBus.Subscribe(TagName.Ball, TagName.LaserFence,
				(ball, laserFence,collision) =>
				{
					var ballCore = ball.GetComponent<Balls.BallCore>();

					TF.ObjectPool.Alloc(_vfx, 1f, ball.transform.position);
					GlobalAudioSource.PlayOneShot(_se);

					ballCore.Kill();
				});
		}
	}
}