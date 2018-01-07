using System.Collections;
using System.Collections.Generic;
using Assets.Damages;
using UnityEngine;

namespace Assets.EnemyBullets
{
	public class EnemyBulletCore : Cores.BaseCore, Damages.IKillable
	{
		//! ----events----
		public event OnKilledHandler onKilled = delegate { };

		//! ----parameters----
		[SerializeField]
		float _damage;
		[SerializeField]
		AudioClip _hitSE;

		//! ----properties----
		public float damage { get { return _damage; } }

		//! ----functions----
		void OnCollisionEnter2D(Collision2D collision)
		{
			var tag = collision.gameObject.GetEnumTagName();

			if (tag == TagName.LaserFence)
				Kill();
			if (tag == TagName.Player)
			{
				var player = collision.gameObject.GetComponent<Players.PlayerCore>();
				player.ApplyDamage(damage);
				GlobalAudioSource.PlayOneShot(_hitSE);
				Kill();
			}
		}

		public void Kill()
		{
			onKilled();
			TF.ObjectPool.Free(gameObject);
		}

		//! ----overrides----
		public override void Clean()
		{
			base.Clean();
			onKilled = delegate { };
		}
	}
}