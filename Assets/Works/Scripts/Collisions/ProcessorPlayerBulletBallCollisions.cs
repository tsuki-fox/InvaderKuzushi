using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerBulletBallCollisions : MonoBehaviour
	{
		//! ----parameters----
		[SerializeField]
		GameObject _effect;

		void Awake()
		{
			CollisionBus.Subscribe(
				new CollisionBus.Combo(TagName.PlayerBullet, TagName.Ball),
				(playerBullet, ball, collision) =>
				{
					var efc = TF.ObjectPool.Alloc(_effect, 1f);
					efc.transform.position = collision.contacts[0].point;

					playerBullet.GetComponent<PlayerBullets.PlayerBulletCore>().Kill();
				});
		}
	}
}