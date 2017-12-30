using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Damages
{
	public delegate void OnDamagedHandler(float damageTaken);
	public delegate void OnDeadHandler();

	public interface IDamageable
	{
		event OnDamagedHandler onDamaged;
		event OnDeadHandler onDead;
		void ApplyDamage(float damageTaken);
	}
}