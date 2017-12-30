using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Players
{
	public class PlayerLogger : BasePlayerComponent
	{
		//! --------parameters--------

		//! --------internal variables--------
		List<string> _logs = new List<string>();
		long _frameCount = 0;

		//! --------functions--------
		void Log(string text)
		{
			var frameText = string.Format("[{0:0000}F]:", _frameCount);
			_logs.Insert(0, frameText + text);
		}
		void Log(string format, params object[] args)
		{
			var text = string.Format(format, args);
			var frameText = string.Format("[{0:0000}F]:", _frameCount);
			_logs.Insert(0, frameText + text);
		}

		//! --------life cycles--------
		void Start()
		{
			core.onInitialized += () =>
			{
				Log("Initialized");
				fireController.onShot += () =>
				 {
					 Log("Shot");
				 };
			};
			core.onCleaned += () =>
			{
				Log("Cleaned");
			};
		}

		void FixedUpdate()
		{
			_frameCount++;
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(PlayerLogger))]
		[CanEditMultipleObjects]
		public class PlayerLoggerInspector : Editor
		{
			Vector2 _scrollPos = new Vector2();

			void Awake()
			{
				EditorApplication.update += Repaint;
			}

			void OnDestroy()
			{
				EditorApplication.update -= Repaint;
			}

			public override void OnInspectorGUI()
			{
				var self = target as PlayerLogger;
				bool flip = true;

				_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

				foreach (var log in self._logs)
				{
					var rect = EditorGUILayout.BeginHorizontal();
					Color color = Color.white;
					EditorGUI.DrawRect(rect, color);
					EditorGUILayout.LabelField(log);
					EditorGUILayout.EndHorizontal();

					flip = !flip;
				}

				EditorGUILayout.EndScrollView();
			}
		}
#endif
	}
}