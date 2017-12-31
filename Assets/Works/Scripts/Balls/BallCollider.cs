using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public class BallCollider : BaseBallComponent
	{
		//! ----delegates----
		public delegate void OnHitWallHandler(Collision2D collision);
		public delegate void OnHitEnemyHandler(Enemies.EnemyCore enemy, Collision2D collision);
		public delegate void OnHitLaserFenceHandler(Collision2D collision);
		public delegate void OnHitPlayerHandler(Players.PlayerCore player,Collision2D collision);

		//! ----events----
		public event OnHitWallHandler onHitWall = delegate { };
		public event OnHitEnemyHandler onHitEnemy = delegate { };
		public event OnHitLaserFenceHandler onHitLaserFence = delegate { };
		public event OnHitPlayerHandler onHitPlayer = delegate { };

		//! ----collisions
		void OnCollisionEnter2D(Collision2D collision)
		{
			var tag = collision.gameObject.GetEnumTagName();

			if (tag == TagName.Wall)
				onHitWall(collision);
			if (tag == TagName.Enemy)
				onHitEnemy(collision.gameObject.GetComponent<Enemies.EnemyCore>(), collision);
			if (tag == TagName.LaserFence)
			{
				onHitLaserFence(collision);
				//? violated the principle!!!
				core.Kill();
			}
			if (tag == TagName.Player)
				onHitPlayer(collision.gameObject.GetComponent<Players.PlayerCore>(), collision);
		}

		//! ----life cycles----
		void Start()
		{
			core.onCleaned += () =>
			{
				onHitWall = delegate { };
				onHitEnemy = delegate { };
				onHitLaserFence = delegate { };
				onHitPlayer = delegate { };
			};
		}
	}
}