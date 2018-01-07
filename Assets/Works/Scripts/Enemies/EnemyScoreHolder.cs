using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
	public class EnemyScoreHolder : BaseEnemyComponent
	{
		//! ----parameters----
		[SerializeField]
		float _score;

		//! ----override----
		public override void Initialize()
		{
			base.Initialize();
			core.onDead += () =>
			{
				ScoreManager.AddScore(_score);
			};
		}
	}
}