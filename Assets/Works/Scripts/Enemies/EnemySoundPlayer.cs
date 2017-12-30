using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
	public class EnemySoundPlayer : BaseEnemyComponent
	{
		//! ----parameters----
		[SerializeField]
		AudioClip _deadSE;

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
			GlobalAudioSource.PlayOneShot(_deadSE);
		}
	}
}