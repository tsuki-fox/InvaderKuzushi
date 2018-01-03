using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Cores
{
	public class Logger : MonoBehaviour
	{
		//! ----type declares----
		public class Message
		{
			public Color color;
			public string text;
			public Message(Color color,string text)
			{
				this.color = color;
				this.text = text;
			}
		}

		//! --------parameters--------

		//! --------internal variables--------
		List<Message> _logs = new List<Message>();
		long _frameCount = 0;

		//! --------functions--------
		public void Log(string text)
		{
			var frameText = string.Format("[{0:0000}F]:", _frameCount);
			var message = new Message(Color.white, frameText + text);

			DOTween.To(
			getter: () => message.color,
			setter: color => message.color = color,
			endValue: Color.grey,
			duration: 1f);

			_logs.Insert(0, message);
		}
		public void Log(string format, params object[] args)
		{
			var text = string.Format(format, args);
			var frameText = string.Format("[{0:0000}F]:", _frameCount);
			var message = new Message(Color.white, frameText + text);

			DOTween.To(
			getter: () => message.color,
			setter: color => message.color = color,
			endValue: Color.grey,
			duration: 1f);

			_logs.Insert(0, message);
		}

		//! --------life cycles--------
		void FixedUpdate()
		{
			_frameCount++;
		}

		public void Clean()
		{
			_frameCount = 0;
			_logs = new List<Message>();
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(Logger))]
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
				var self = target as Logger;
				bool flip = true;

				_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

				foreach (var log in self._logs)
				{
					var rect = EditorGUILayout.BeginHorizontal();
					Color color = log.color;
					EditorGUI.DrawRect(rect, color);
					EditorGUILayout.LabelField(log.text);
					EditorGUILayout.EndHorizontal();
					flip = !flip;
				}

				EditorGUILayout.EndScrollView();
			}
		}
#endif
	}
}