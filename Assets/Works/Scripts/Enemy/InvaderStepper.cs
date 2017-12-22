using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InvaderStepper : MonoBehaviour
{
	//! --------paramaters--------
	[SerializeField]
	float _moveTick = 0.5f;
	[SerializeField]
	float _moveStepX = 0.2f;
	[SerializeField]
	float _moveStepY = 0.5f;

	bool _locked = false;
	public bool locked
	{
		get { return _locked; }
		set { _locked = value; }
	}

	//! --------internal variables--------
	BoxCollider2D _collider;
	float _pauseTime = 0f;
	bool _isRight = true;

	//! --------functions--------
	void Move()
	{
		float step = _isRight ? _moveStepX : -_moveStepX;
		Vector2 center = _collider.transform.position.ToVector2() + _collider.offset;
		Vector2 size = _collider.bounds.size;

		var hit = Physics2D.BoxCast(center, size, 0f, Vector2.right, step, LayerMask.GetMask("Wall"));
		if (!hit)
			transform.AddPositionX(step);
		else
		{
			transform.AddPositionY(_moveStepY);
			_isRight = !_isRight;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float step = _isRight ? _moveStepX : -_moveStepX;

		if(_collider)
			Handles.DrawWireCube(_collider.transform.position + _collider.offset.ToVector3()+new Vector2(step,0).ToVector3(), _collider.bounds.size);
	}
#endif

	void Awake()
	{
		_collider = GetComponentInChildren<BoxCollider2D>();
		if (!_collider)
			Debug.LogWarning("BoxCollider2D not found");
	}

	void Update()
	{
		if (_locked)
			return;
		_pauseTime += Time.deltaTime;
		if(_pauseTime>=_moveTick)
		{
			Move();
			_pauseTime = 0f;
		}
	}
}
