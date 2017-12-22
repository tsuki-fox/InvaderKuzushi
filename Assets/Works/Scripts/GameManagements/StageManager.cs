using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StageManager : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	List<string> _organizeName = new List<string>();
	[SerializeField]
	float _tryInterval = 1f;

	//! --------internal variables--------
	EnemyEmitter _emitter;
	int _organizeCount = 0;

	[Extractable]
	bool _isExtinction;

	//! --------functions--------
	bool IsExtinction()
	{
		var enemy = GameObject.FindGameObjectWithTag(TagName.Enemy.ToString());
		return enemy == null;
	}

	void TryNext()
	{
		if(_organizeCount==_organizeName.Count)
		{
			//
		}

		_isExtinction = IsExtinction();

		if(IsExtinction())
		{
			_organizeCount++;
			_emitter.Emit(_organizeName[_organizeCount]);
		}
	}

	void Awake()
	{
		_emitter = FindObjectOfType<EnemyEmitter>();
	}

	void Start()
	{
		_emitter.Emit(_organizeName[0]);

		Observable.Interval(TimeSpan.FromSeconds(_tryInterval)).Subscribe(_ =>
		{
			TryNext();
		}).AddTo(gameObject);
	}

	void Update()
	{

	}
}
