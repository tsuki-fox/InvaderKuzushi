using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public class BallMover : BaseBallComponent
	{
		//! ----parameters----
		[SerializeField]
		float _speedLimitMin;
		[SerializeField]
		float _speedLimitMax;

		//! ----functions----
		void VelocityControl()
		{
			float mag = rigidbody2D.velocity.magnitude;
			Vector2 dir = rigidbody2D.velocity.normalized;

			if (mag > _speedLimitMax)
				rigidbody2D.velocity = _speedLimitMax * dir;
			if (mag < _speedLimitMin)
				rigidbody2D.velocity = _speedLimitMin * dir;
		}

		//! ----life cycles----
		void LateUpdate()
		{
			VelocityControl();
		}
	}
}
