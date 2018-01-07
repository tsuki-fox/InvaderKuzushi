using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Players
{
	public class PlayerSoundPlayer : BasePlayerComponent
	{
		//! --------parameters--------
		[SerializeField]
		AudioClip _fireSE;
		[SerializeField]
		AudioClip _missileSE;
		[SerializeField]
		AudioClip _deadSE;

		//! ----functions----
		public override void Initialize()
		{
			base.Initialize();
			fireController.onShot += () =>
			{
				GlobalAudioSource.PlayOneShot(_fireSE);
			};
			missileController.onShot += () =>
			{
				GlobalAudioSource.PlayOneShot(_missileSE);
			};
			core.onDead += () =>
			{
				GlobalAudioSource.PlayOneShot(_deadSE);
			};
		}
	}
}