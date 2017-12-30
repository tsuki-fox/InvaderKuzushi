using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Damages
{
	public delegate void OnKilledHandler();
	public interface IKillable
	{
		event OnKilledHandler onKilled;
		void Kill();
	}
}