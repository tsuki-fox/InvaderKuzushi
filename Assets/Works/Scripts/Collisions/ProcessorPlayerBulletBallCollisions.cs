using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorPlayerBulletBallCollisions :BaseProcessor
	{
		[Header("プレイヤー弾-ボール 衝突処理")]
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