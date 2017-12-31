using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

public class LifeCycleRuler : MonoBehaviour
{
	[SerializeField]
	float _scoreByEnemy;
	[SerializeField]
	GameObject _explosionVFX;
	[SerializeField]
	AudioClip _explosionSE;

	void Awake()
	{
		LifeCycleBus.Subscribe(TagName.Enemy, enemy =>
		{
			var exp = ObjectPool.Alloc(_explosionVFX);
			exp.transform.position = enemy.transform.position;
			ObjectPool.Free(exp, 1f);
			GlobalAudioSource.PlayOneShot(_explosionSE);
			ScoreManager.AddScore(_scoreByEnemy);
		});
	}

	void OnDestroy()
	{
		LifeCycleBus.Clean();	
	}
}
