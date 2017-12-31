using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public class BallAttacker : BaseBallComponent
	{
		//! ----parameters----
		[SerializeField]
		float _damage;

		//! ----life cycles----
		void Start()
		{
			core.onInitialized += () =>
			{
				collider.onHitEnemy += Collider_onHitEnemy;
			};
		}

		private void Collider_onHitEnemy(Enemies.EnemyCore enemy, Collision2D collision)
		{
			enemy.ApplyDamage(_damage);
		}
	}
}