using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	float _speed;
	[SerializeField]
	float _speedLimitMin;
	[SerializeField]
	float _speedLimitMax;
	[SerializeField]
	Vector2 _power;

	//! --------internal variables--------
	Rigidbody2D _rigidbody;

	//! --------functions--------
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetEnumTagName() == TagName.LaserFence)
			BallActivityBus.NotifyDrop(gameObject);
	}

	void VelocityControl()
	{
		float mag = _rigidbody.velocity.magnitude;
		Vector2 dir = _rigidbody.velocity.normalized;

		if (mag > _speedLimitMax)
			_rigidbody.velocity = _speedLimitMax * dir;
		if (mag < _speedLimitMin)
			_rigidbody.velocity = _speedLimitMin * dir;
	}

	void Boost()
	{
		_rigidbody.velocity = _power;
	}

	void BoostRandom()
	{
		_rigidbody.velocity = RandomEx.RangeVector2(Vector2.zero, Vector2.one) * 3f;
	}

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void LateUpdate()
	{
		VelocityControl();	
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(BallController))]
	[CanEditMultipleObjects]
	public class BallContollerInspector:Editor
	{
		public override void OnInspectorGUI()
		{
			var self = target as BallController;

			if (GUILayout.Button("Boost"))
				self.Boost();
			if (GUILayout.Button("RandomBoost"))
				self.BoostRandom();
			base.OnInspectorGUI();
		}
	}
#endif
}
