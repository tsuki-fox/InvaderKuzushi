using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerBulletWallCollisions : MonoBehaviour
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
				new CollisionBus.Combo(TagName.PlayerBullet, TagName.Wall),
				(bullet, wall, collision) =>
				{
					var core = bullet.GetComponent<PlayerBullets.PlayerBulletCore>();

					var effect=TF.ObjectPool.Alloc(_effect);
					TF.ObjectPool.Free(effect, 1f);
					if(collision.contacts.Length!=0)
						effect.transform.position = collision.contacts[0].point;
	
					GlobalAudioSource.PlayOneShot(_hitSE);

					core.Kill();
				});
		}
	}
}