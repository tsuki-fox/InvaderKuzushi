using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;
using DG.Tweening;
using UniRx;

public class CollisionRuler : MonoBehaviour
{
	[SerializeField]
	float _ballDamage;
	[SerializeField]
	float _bulletDamage;
	[SerializeField]
	AudioClip _wallHitSE;
	[SerializeField]
	AudioClip _barHitSE;
	[SerializeField]
	AudioClip _bulletHitSE;
	[SerializeField]
	GameObject _bulletHitVFX;
	[SerializeField]
	GameObject _ball;
	[SerializeField]
	GameObject _ballSpawnVFX;
	[SerializeField]
	GameObject _ballDieVFX;
	[SerializeField]
	AudioClip _ballDieSE;

	void Awake()
	{
		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.Enemy, TagName.Ball, (enemy, ball, collision) =>
		{
			var killable = enemy.GetComponent<Killable>();
			killable.TakeDamage(_ballDamage);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.Wall, TagName.Ball, (wall, ball, collision) =>
		{
			GlobalAudioSource.PlayOneShot(_wallHitSE);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.Player, TagName.Ball, (player, ball, collision) =>
		{
			GlobalAudioSource.PlayOneShot(_barHitSE);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.PlayerBullet, TagName.Ball, (bullet, ball, collision) =>
		{
			GlobalAudioSource.PlayOneShot(_barHitSE);
			var vfx = ObjectPool.Alloc(_bulletHitVFX);
			vfx.transform.position = bullet.transform.position;
			ObjectPool.Free(vfx, 1f);
			ObjectPool.Free(bullet);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.PlayerBullet, TagName.Enemy, (bullet, enemy, collision) =>
		{
			var killable = enemy.GetComponent<Killable>();
			killable.TakeDamage(_bulletDamage);
			GlobalAudioSource.PlayOneShot(_bulletHitSE);
			var vfx = ObjectPool.Alloc(_bulletHitVFX);
			vfx.transform.position = bullet.transform.position;
			ObjectPool.Free(vfx, 1f);
			ObjectPool.Free(bullet);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.LaserFence, TagName.Ball, (fence, ball, collision) =>
		{
			var vfx = ObjectPool.Alloc(_ballDieVFX);
			vfx.transform.position = ball.transform.position;
			ObjectPool.Free(vfx, 1f);
			ObjectPool.Free(ball);

			GlobalAudioSource.PlayOneShot(_ballDieSE);

			Observable.Timer(System.TimeSpan.FromSeconds(1f)).Subscribe( _ =>
			 {
				 var newBall = ObjectPool.Alloc(_ball);
				 var spawnVFX = ObjectPool.Alloc(_ballSpawnVFX);
				 newBall.transform.position = Vector2.zero;
				 spawnVFX.transform.position = newBall.transform.position;
				 ObjectPool.Free(spawnVFX, 1f);
			 });

			Camera.main.transform.DOComplete();
			Camera.main.transform.DOShakePosition(0.5f, 0.3f);
		});
	}

	void OnDestroy()
	{
		CollisionBus.Clean(); 
	}
}
