using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Assets.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScoreManager : MonoBehaviour
{
	static ScoreManager _self = null;

	[Extractable]
	static float _topScore = 0f;

	static float _rawScore;
	[Extractable]
	static float _displayedScore;
	[SerializeField]
	float _completeTime = 0.5f;

	static Tweener _tweener;

	[Extractable]
	public float buffer
	{
		get
		{
			return _rawScore - _displayedScore;
		}
	}

	void Awake()
	{
		if (_self == null)
			_self = this;
	}

	void Start()
	{
		SceneConductor.onPlaySceneStart += () =>
		{
			_rawScore = 0f;
			_displayedScore = 0f;
		};
		SceneConductor.onPlaySceneEnd += () =>
		{
			if (_topScore < _rawScore)
				_topScore = _rawScore;
		};
		SceneConductor.onResultSceneStart += () =>
		{
			Reanimation();
		};
	}

	void OnDestroy()
	{
		if(_tweener!=null)
			_tweener.Complete();
	}

	static void Initialize()
	{
		if (_self == null)
			_self = FindObjectOfType<ScoreManager>();
	}
	static bool CheckInitialize()
	{
		Initialize();
		return _self == null ? false : true;
	}

	public static void AddScore(float value)
	{
		CheckInitialize();
		_rawScore += value;
		_tweener=DOTween.To(
			() => _displayedScore,
			num => _displayedScore = num,
			_rawScore,
			_self._completeTime);
	}

	public static void Reanimation()
	{
		_displayedScore = 0;
		_tweener = DOTween.To(
			() => _displayedScore,
			num => _displayedScore = num,
			_rawScore,
			_self._completeTime);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ScoreManager))]
	public class ScoreManagerInspector:Editor
	{
		public override void OnInspectorGUI()
		{
			if(GUILayout.Button("AddScore"))
			{
				ScoreManager.AddScore(1000);
			}
			if (GUILayout.Button("Re"))
				Reanimation();
		}
	}
#endif
}
