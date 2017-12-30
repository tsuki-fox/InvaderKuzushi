using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

namespace Assets.PlayerBullets
{
	public class PlayerBulletEffectEmitter : BasePlayerBulletComponent
	{
		//! ----parameters----
		[SerializeField]
		GameObject _attackedVFX;

		//! ----life cycles----
		void Start()
		{
			core.onInitialized += () =>
			{
				attacker.onHitted += Attacker_onHitted;
			};
		}

		private void Attacker_onHitted(Collision2D collision)
		{
			var pos = transform.position;
			var vfx = ObjectPool.Alloc(_attackedVFX);
			vfx.transform.position = pos;
			ObjectPool.Free(vfx, 1f);
		}
	}
}