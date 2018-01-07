using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorBallEnemyCollisions : BaseProcessor
	{
		[Header("ボール-敵 衝突判定")]
		//! ----parameters----
		[SerializeField]
		GameObject _hitVFX;
		[SerializeField]
		AudioClip _hitSE;

		//! ----functions----
		void Awake()
		{
			CollisionBus.Subscribe(TagName.Ball, TagName.Enemy,
				(ball, enemy, collision) =>
				{
					var ballCore = ball.GetComponent<Balls.BallCore>();
					var enemyCore = enemy.GetComponent<Enemies.EnemyCore>();

					TF.ObjectPool.Alloc(_hitVFX, 1f, collision.contacts[0].point);
					GlobalAudioSource.PlayOneShot(_hitSE);

					enemyCore.ApplyDamage(ballCore.damage);
				}); 
		}
	}
}