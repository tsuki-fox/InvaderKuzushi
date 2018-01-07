using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Missiles
{
	public class MissileSeeker : BaseMissileComponent
	{
		//! ----static----
		static List<GameObject> _locked = new List<GameObject>();

		//! ----delegates----
		public delegate void OnLockedHandler(GameObject enemy);

		//! ----events----
		public event OnLockedHandler onLocked = delegate { };

		//! ----parameters----
		[SerializeField]
		float _speed = 3f;
		[SerializeField]
		float _angleDeltaLimit = 5f;

		//! ----internal variables----
		GameObject _target = null;

		//! ----functions----
		GameObject LockOn()
		{
			var hit = Physics2D.CircleCast(
				origin: transform.position,
				radius: 100f,
				direction: Vector2.up,
				distance: 0,
				layerMask: LayerMask.GetMask("Enemy"));
			
			if(!hit)
				return null;

			var go = hit.rigidbody.gameObject;
			_locked.Add(go);
			logger.Log("onlocked:{0}", go.name);

			var damageable = go.GetComponent<Damages.IDamageable>();
			damageable.onDead += () =>
			{
				_locked.Remove(go);
				_target = null;
			};

			onLocked(go);
			return go;
		}

		void Search()
		{
		}

		void Manipulate()
		{
			if(_target==null)
			{
				SetVelocity(rigidbody2D.rotation, _speed);
			}
			else
			{
				float angle = rigidbody2D.velocity.ToAngle();
				float targetAngle = (_target.transform.position - transform.position).ToVector2().ToAngle();
				float delta = Mathf.DeltaAngle(angle, targetAngle);
				if (delta > 0)
					SetVelocity(angle + _angleDeltaLimit, _speed);
				else
					SetVelocity(angle - _angleDeltaLimit, _speed);
			}
		}

		void SetVelocity(float angle, float speed)
		{
			Vector2 velocity = new Vector2();
			velocity.x = Mathf.Cos(Mathf.Deg2Rad * angle) * speed;
			velocity.y = Mathf.Sin(Mathf.Deg2Rad * angle) * speed;
			rigidbody2D.velocity = velocity;
		}

		//! ----overrides----

		public override void Prepare()
		{
			base.Prepare();
			_locked = new List<GameObject>();
		}
		public override void Clean()
		{
			base.Clean();
			onLocked = delegate { };
		}
		void Update()
		{
			if (_target == null)
				_target = LockOn();
			Manipulate();
		}
	}
}