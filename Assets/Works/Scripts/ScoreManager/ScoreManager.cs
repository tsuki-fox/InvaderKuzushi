using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScoreManager : MonoBehaviour
{
	static ScoreManager _self = null;

	float _rawScore;
	[Extractable]
	float _displayedScore;
	[SerializeField]
	float _completeTime = 0.5f;

	void Awake()
	{
		if (_self == null)
			_self = this;
	}

	public static void AddScore(float value)
	{
		_self._rawScore += value;
		DOTween.To(
			() => _self._displayedScore,
			num => _self._displayedScore = num,
			_self._rawScore,
			_self._completeTime);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ScoreManager))]
	public class ScoreManagerInspector:Editor
	{
		public override void OnInspectorGUI()
		{
			var self = target as ScoreManager;

			if(GUILayout.Button("AddScore"))
			{
				ScoreManager.AddScore(1000);
			}
		}
	}
#endif
}
