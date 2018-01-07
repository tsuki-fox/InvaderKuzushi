using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TF;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	float _speed;
	[SerializeField]
	GameObject _sourceBullet;
	[SerializeField]
	float _shotVelocity;
	[SerializeField]
	int _maxBallLife = 3;
	[SerializeField]
	Color _lifesColor;
	[SerializeField]
	Color _lostLifesColor;
	[SerializeField]
	GameObject _sourceMissile;
	[SerializeField]
	AudioClip _missileSE;

	//! --------internal variables--------
	Rigidbody2D _rigidbody;
	int _ballLife;
	List<SpriteRenderer> _lifeIconSprites;
	PowerLevelManager _powerLevelManager;

	//! --------properties--------
	public int ballLife
	{
		get { return _ballLife;}
		set
		{
			_ballLife = value;
		}
	}

	//! --------functions--------
	void Initialize()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_ballLife = _maxBallLife;
		BallActivityBus.onDrop += (ball) =>
		{
			ballLife--;
			if (_ballLife < 0)
				GameActivityBus.NotifyGameOver();
		};

		_powerLevelManager = GetComponent<PowerLevelManager>();
	}

	void AdjustLifeIconsColor()
	{
		foreach(var icon in _lifeIconSprites)
			icon.color = _lostLifesColor;
		for (int i = 0; i < _ballLife; i++)
			_lifeIconSprites[i].color = _lifesColor;
	}

	void Move()
	{
		var velocity = new Vector2();

		if (MyInput.left)
			velocity.x -= _speed;
		if (MyInput.right)
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

			SpecialShot();
		}
	}

	void SpecialShot()
	{
		if (_powerLevelManager.isFullCharge)
			_powerLevelManager.level = 0;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetEnumTagName()== TagName.Ball)
			_powerLevelManager.level++;
	}

	void Awake()
	{
		Initialize();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Z))
		{
			for (int f1 = 0; f1 <4; f1++)
			{
				var missile = ObjectPool.Alloc(_sourceMissile);
				missile.transform.position = transform.position;
				GlobalAudioSource.PlayOneShot(_missileSE);
			}
		}

		Move();
		Shot();
	}
}
