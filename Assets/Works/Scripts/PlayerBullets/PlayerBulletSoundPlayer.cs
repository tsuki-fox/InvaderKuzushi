using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerBullets
{
	public class PlayerBulletSoundPlayer :BasePlayerBulletComponent
	{
		//! ----parameters----
		[SerializeField]
		AudioClip _hitSE;

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
			GlobalAudioSource.PlayOneShot(_hitSE);
		}
	}
}