using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class ProcessorBallWallCollisions : BaseProcessor
	{
		[Header("ボール-壁 衝突処理")]
		//! ----parameters----
		[SerializeField]
		GameObject _vfx;
		[SerializeField]
		AudioClip _se;

		//! ----functions----
		void Awake()
		{
			CollisionBus.Subscribe(new CollisionBus.Combo(TagName.Ball, TagName.Wall),
				(ball, wall, collision) =>
				{
					TF.ObjectPool.Alloc(_vfx, 1f, collision.contacts[0].point);
					GlobalAudioSource.PlayOneShot(_se);
				});
		}
	}
}