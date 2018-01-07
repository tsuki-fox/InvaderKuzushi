using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Cores
{
	[RequireComponent(typeof(Cores.Logger))]
	public abstract class BaseComponent : MonoBehaviour
	{
		//! ----internal variables----
		Cores.Logger _logger;

		//! ----properties----
		public Cores.Logger logger { get { return _logger; } }
		
		//! ----functions----
		public virtual void Prepare()
		{
			_logger = GetComponent<Logger>();
		}
		public virtual void Initialize() { }
		public virtual void Clean() { }
		public virtual void Release() { }
	}
}