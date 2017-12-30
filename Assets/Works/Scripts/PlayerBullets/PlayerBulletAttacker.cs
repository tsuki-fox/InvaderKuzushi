using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerBullets
{
	public class PlayerBulletAttacker : BasePlayerBulletComponent
	{
		//! ----parameters----
		[SerializeField]
		float _damage;

		//! ----delegate----
		public delegate void OnAttackedHandler(Enemies.EnemyCore enemy,Collision2D collision,float damage);
		public delegate void OnHittedHandler(Collision2D collision);

		//! ----events----
		public event OnAttackedHandler onAttacked = delegate { };
		public event OnHittedHandler onHitted = delegate { };

		//! ----collisions----
		void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.GetEnumTagName() == TagName.Enemy)
			{
				var enemy = collision.gameObject.GetComponent<Enemies.EnemyCore>();
				enemy.ApplyDamage(_damage);
				onAttacked(enemy, collision, _damage);
			}
			onHitted(collision);
			core.Kill();
		}

		//! ----life cycles----
		void Start()
		{
			core.onCleaned += () =>
			{
				onAttacked = delegate { };
				onHitted = delegate { };
			};
		}
	}
}
