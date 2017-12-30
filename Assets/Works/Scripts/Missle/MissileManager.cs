using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class MissileManager : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	static float _refleshTime = 1f;

	//! --------internal variables--------
	static GameObject[] _enemies = null;
	static List<GameObject> _locked = new List<GameObject>();

	//! --------functions--------
	static bool IsLocked(GameObject enemy)
	{
		return _locked.Find(item => item == enemy) != null;
	}
	static void Lock(GameObject enemy)
	{
		_locked.Add(enemy);
	}
	public static void Unlock(GameObject enemy)
	{
		_locked.Remove(enemy);
	}

	public static void Reflesh()
	{
		_enemies = GameObject.FindGameObjectsWithTag(TagName.Enemy.ToString());
	}

	public static GameObject Lockon(Vector2 pos)
	{
		if (_enemies == null)
			return null;

		var target = _enemies
			.Where(enemy => !IsLocked(enemy))
			.FindMin(enemy => Vector2.Distance(enemy.transform.position.ToVector2(), pos));

		if (target == null)
			target = _enemies.FindMin(enemy => Vector2.Distance(enemy.transform.position.ToVector2(), pos));

		if (target != null)
			_locked.Add(target);

		return target;
	}

	void Awake()
	{
		Observable.Interval(System.TimeSpan.FromSeconds(_refleshTime)).Subscribe(_ =>
		{
			Reflesh();
		//	Debug.LogFormat("reflesh search {0} enemies", _enemies.Count());
		}).AddTo(gameObject);
	}
}