using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

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
			ObjectPool.Free(bullet);
		});

		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.PlayerBullet, TagName.Enemy, (bullet, enemy, collision) =>
		{
			var killable = enemy.GetComponent<Killable>();
			killable.TakeDamage(_bulletDamage);
			GlobalAudioSource.PlayOneShot(_bulletHitSE);
			ObjectPool.Free(bullet);
		});
	}

	void OnDestroy()
	{
		CollisionBus.Clean(); 
	}
}
