using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TF;

public class MissileController : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	float _researchTime = 0.3f;
	[SerializeField]
	float _speed = 3f;
	[SerializeField]
	GameObject _smokeVFX;
	[SerializeField]
	float _smokeInterval;
	[SerializeField]
	GameObject _lockonMakerVFX;

	//! --------internal variables--------
	Rigidbody2D _rigidbody;
	GameObject _target = null;

	//! --------functions--------
	void Search()
	{
		_target = MissileManager.Lockon(transform.position);
		if(_target!=null)
		{
			var marker = ObjectPool.Alloc(_lockonMakerVFX);
			marker.transform.position = _target.transform.position;
			ObjectPool.Free(marker, 1f);
		}
	}

	void SetVelocity(float angle,float speed)
	{
		Vector2 velocity = new Vector2();
		velocity.x = Mathf.Cos(Mathf.Deg2Rad * angle) * speed;
		velocity.y = Mathf.Sin(Mathf.Deg2Rad * angle) * speed;
		_rigidbody.velocity = velocity;
	}

	void Manipulate()
	{
		float angle = _rigidbody.velocity.ToAngle();
		float targetAngle = (_target.transform.position-transform.position).ToVector2().ToAngle();
		float delta = Mathf.DeltaAngle(angle, targetAngle);
		if (delta > 0)
			SetVelocity(angle + 5, _speed);
		else
			SetVelocity(angle - 5, _speed);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetEnumTagName() == TagName.Enemy)
		{
			MissileManager.Unlock(_target);
			ObjectPool.Free(gameObject);
			_target = null;
		}
	}

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		Observable.Interval(System.TimeSpan.FromSeconds(_researchTime)).Subscribe(_ =>
		{
			if (!gameObject.activeSelf)
				return;

			if (_target == null)
				Search();
		}).AddTo(gameObject);

		Observable.Interval(System.TimeSpan.FromSeconds(_smokeInterval)).Subscribe(_ =>
		{
			if (!gameObject.activeSelf)
				return;

			var smoke = ObjectPool.Alloc(_smokeVFX);
			smoke.transform.position = transform.position;
			ObjectPool.Free(smoke, 1f);
		}).AddTo(gameObject);
	}

	void Update()
	{
		if (_target != null)
			Manipulate();
	}
}
