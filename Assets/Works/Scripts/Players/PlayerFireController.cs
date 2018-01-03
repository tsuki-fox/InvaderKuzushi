using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

namespace Assets.Players
{
	public class PlayerFireController : BasePlayerComponent
	{
		//! --------parameters--------
		[SerializeField]
		GameObject _bulletSource;
		[SerializeField]
		float _shotVelocity;

		//! --------delegate declares--------
		public delegate void OnShotHandler();

		//! --------events-------
		public event OnShotHandler onShot = delegate { };

		//! --------functions--------
		void TryShot()
		{
			if(MyInput.downShot)
			{
				var bullet = ObjectPool.Alloc(_bulletSource);
				bullet.transform.position = transform.position;
				var bulletCore = bullet.GetComponent<PlayerBullets.PlayerBulletCore>();
				var bulletBody = bullet.GetComponent<Rigidbody2D>();
				bulletBody.velocity = new Vector2(0f, _shotVelocity);
				onShot();
			}
		}

		public override void Clean()
		{
			base.Clean();
			onShot = delegate { };
		}

		//! --------life cycles--------
		void Update()
		{
			TryShot();
		}
	}
}