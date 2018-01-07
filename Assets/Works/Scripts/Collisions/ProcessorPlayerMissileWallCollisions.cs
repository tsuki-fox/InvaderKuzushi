using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerMissileWallCollisions : BaseProcessor
	{
		[Header("ミサイル-壁 衝突処理")]
		//! ----parameters----
		[SerializeField]
		GameObject _vfx;
		[SerializeField]
		AudioClip _se;

		//! ----functions----
		void Awake()
		{
			CollisionBus.Subscribe(new CollisionBus.Combo(TagName.PlayerMissile, TagName.Wall),
				(missile, wall, collision) =>
				{
					var missileCore = missile.GetComponent<Missiles.MissileCore>();
					TF.ObjectPool.Alloc(_vfx, 1f, collision.contacts[0].point);
					GlobalAudioSource.PlayOneShot(_se);
					missileCore.Kill();
				});
		}
	}
}