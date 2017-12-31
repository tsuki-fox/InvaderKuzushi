using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public class BallSoundPlayer : BaseBallComponent
	{
		//! ----parameters----
		[SerializeField]
		AudioClip _hitWallSE;
		[SerializeField]
		AudioClip _hitEnemySE;
		[SerializeField]
		AudioClip _hitPlayerSE;
		[SerializeField]
		AudioClip _fallenSE;

		//! ----life cycles----
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
			GlobalAudioSource.PlayOneShot(_hitPlayerSE);
		}

		private void Collider_onHitLaserFence(Collision2D collision)
		{
			GlobalAudioSource.PlayOneShot(_fallenSE);
		}

		private void Collider_onHitEnemy(Enemies.EnemyCore enemy, Collision2D collision)
		{
			GlobalAudioSource.PlayOneShot(_hitEnemySE);
		}

		private void Collider_onHitWall(Collision2D collision)
		{
			GlobalAudioSource.PlayOneShot(_hitWallSE);
		}
	}
}