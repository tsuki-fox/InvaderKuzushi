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

		//! --------life cycles--------
		void Start()
		{
			core.onInitialized += () =>
			{
				fireController.onShot += () =>
				{
					GlobalAudioSource.PlayOneShot(_fireSE);
				};

				missileController.onShot += () =>
				{
					GlobalAudioSource.PlayOneShot(_missileSE);
				};
			};
		}
	}
}