using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerBulletEnemyCollisions : MonoBehaviour
	{
		//! ----parameters----
		[SerializeField]
		GameObject _effect;
		[SerializeField]
		AudioClip _hitSE;

		//! ----life cycles----
		void Awake()
		{
			Collisions.CollisionBus.Subscribe(
				new CollisionBus.Combo(TagName.PlayerBullet, TagName.Enemy),
				(playerBullet, enemy, collision) =>
				{
					var enemyCore = enemy.GetComponent<Enemies.EnemyCore>();
					var playerBulletCore = playerBullet.GetComponent<PlayerBullets.PlayerBulletCore>();

					enemyCore.ApplyDamage(playerBulletCore.damage);

					var effect = TF.ObjectPool.Alloc(_effect);
					if(collision.contacts.Length!=0)
						effect.transform.position = collision.contacts[0].point;
					TF.ObjectPool.Free(effect, 1f);

					GlobalAudioSource.PlayOneShot(_hitSE);
					playerBulletCore.Kill();
				});
		}
	}
}