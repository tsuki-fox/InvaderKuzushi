using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

namespace Assets.Enemies
{
	public class EnemyEffectEmitter : BaseEnemyComponent
	{
		//! ----parameters----
		[SerializeField]
		GameObject _deadVFX;

		//! ----life cycles----
		void Start()
		{
			core.onInitialized += () =>
			{
				core.onDead += Core_onDead;
			};
		}

		private void Core_onDead()
		{
			var vfx = ObjectPool.Alloc(_deadVFX);
			vfx.transform.position = transform.position;
			ObjectPool.Free(vfx, 1f);
		}
	}
}