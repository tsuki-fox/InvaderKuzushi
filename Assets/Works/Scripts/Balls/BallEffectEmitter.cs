using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

namespace Assets.Balls
{
	public class BallEffectEmitter : BaseBallComponent
	{
		//! ----parameters----
		[SerializeField]
		GameObject _hitWallVFX;
		[SerializeField]
		GameObject _fallenVFX;
		[SerializeField]
		GameObject _hitEnemyVFX;
		[SerializeField]
		GameObject _hitPlayerVFX;

		//! ----life cycle----
		void Start()
		{
			core.onInitialized += () =>
			{
				collider.onHitWall += Collider_onHitWall;
				collider.onHitEnemy += Collider_onHitEnemy;
				collider.onHitLaserFence += Collider_onHitLaserFence;
				collider.onHitPlayer += Collider_onHitPlayer;
			};
		}

		private void Collider_onHitPlayer(Players.PlayerCore player, Collision2D collision)
		{
			var vfx = ObjectPool.Alloc(_hitPlayerVFX);
			vfx.transform.position = collision.contacts[0].point;
			ObjectPool.Free(vfx, 1f);
		}

		private void Collider_onHitLaserFence(Collision2D collision)
		{
			var vfx = ObjectPool.Alloc(_fallenVFX);
			vfx.transform.position = transform.position;
			ObjectPool.Free(vfx, 1f);
		}

		private void Collider_onHitEnemy(Enemies.EnemyCore enemy, Collision2D collision)
		{
			var vfx = ObjectPool.Alloc(_hitEnemyVFX);
			vfx.transform.position = collision.contacts[0].point;
			ObjectPool.Free(vfx, 1f);
		}

		private void Collider_onHitWall(Collision2D collision)
		{
			var vfx = ObjectPool.Alloc(_hitWallVFX);
			vfx.transform.position = collision.contacts[0].point;
			ObjectPool.Free(vfx, 1f);
		}
	}
}