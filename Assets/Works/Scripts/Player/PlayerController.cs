using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	Rigidbody2D _rigidbody;

	[SerializeField]
	float _speed;
	[SerializeField]
	GameObject _sourceBullet;
	[SerializeField]
	float _shotVelocity;


	void Move()
	{
		var velocity = new Vector2();

		if (MyInput.isLeft)
			velocity.x -= _speed;
		if (MyInput.isRight)
			velocity.x += _speed;
		_rigidbody.velocity = velocity;
	}

	void Shot()
	{
		if (MyInput.downShot)
		{
			var bullet = TF.ObjectPool.Alloc(_sourceBullet);
			bullet.transform.position = transform.position;
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _shotVelocity);
		}
	}

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();	
	}

	void Update()
	{
		Move();
		Shot();
	}
}
