using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerMissileEnemyCollisions : BaseProcessor
	{
		[Header("ミサイル-敵 衝突処理")]
		//! ----parameters----
		[SerializeField]
		GameObject _vfx;
		[SerializeField]
		AudioClip _se;

		//! ----functions----
		void Awake()
		{
			CollisionBus.Subscribe(new CollisionBus.Combo(TagName.PlayerMissile, TagName.Enemy),
				(missile, enemy, collision) =>
				{
					TF.ObjectPool.Alloc(_vfx, 1f, missile.transform.position);
					GlobalAudioSource.PlayOneShot(_se);

					var missileCore = missile.GetComponent<Missiles.MissileCore>();
					var enemyCore = enemy.GetComponent<Enemies.EnemyCore>();
					enemyCore.ApplyDamage(missileCore.damage);
					missileCore.Kill();
				});
		}
	}
}