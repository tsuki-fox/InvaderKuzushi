using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

namespace Assets.Players
{
	public class PlayerMissileController : BasePlayerComponent
	{
		//! --------parameters--------
		[SerializeField,Extractable]
		int _maxPowerLevel;
		[SerializeField]
		GameObject _sourceMissile;
		[SerializeField]
		Vector2 _missileOffset;

		//! --------internal variables--------
		[SerializeField,Extractable]
		int _powerLevel;

		//! --------delegate declares--------
		public delegate void OnPowerUppedHandler(float diff);
		public delegate void OnPowerDownedHandler(float diff);
		public delegate void OnPowerChangedHandler(float diff);
		public delegate void OnShotHandler();

		//! --------events--------
		public event OnPowerUppedHandler onPowerUpped = delegate { };
		public event OnPowerDownedHandler onPowerDowned = delegate { };
		public event OnPowerChangedHandler onPowerChanged = delegate { };
		public event OnShotHandler onShot = delegate { };

		//! --------properties--------
		public int maxPowerLevel { get { return _maxPowerLevel; } }
		public int powerLevel
		{
			get { return _powerLevel; }
			set
			{
				int nextLevel = Mathf.Clamp(value, 1, _maxPowerLevel);
				int diff = nextLevel - _powerLevel;

				if (diff != 0)
					onPowerChanged(diff);
				if (diff > 0)
					onPowerUpped(diff);
				if (diff < 0)
					onPowerDowned(diff);

				_powerLevel = nextLevel;
			}
		}

		//! --------functions--------
		void TryShot()
		{
			if (_powerLevel == _maxPowerLevel)
			{
				var missile = ObjectPool.Alloc(_sourceMissile);
				missile.transform.position = transform.position + _missileOffset.ToVector3();

				_powerLevel = 0;
				onShot();
			}
		}

		//! --------collisions--------
		void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.GetEnumTagName() == TagName.Ball)
			{
				powerLevel++;
			}
		}

		public override void Clean()
		{
			base.Clean();
			onPowerChanged = delegate { };
			onPowerDowned = delegate { };
			onPowerUpped = delegate { };
			onShot = delegate { };
			_powerLevel = 0;
		}

		public override void Initialize()
		{
			base.Initialize();
			_powerLevel = 0;
		}

		//! --------life cycles--------
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				TryShot();
		}

	}
}