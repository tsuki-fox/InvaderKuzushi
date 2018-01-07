using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Players
{
	public class PlayerEffectEmitter : BasePlayerComponent
	{
		//! ----parameters----
		[SerializeField]
		GameObject _deadVFX;

		//! ----overrides----
		public override void Initialize()
		{
			base.Initialize();
			core.onDead += () =>
			{
				TF.ObjectPool.Alloc(_deadVFX, 1f, transform.position);
			};
		}
	}
}