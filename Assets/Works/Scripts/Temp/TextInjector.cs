using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Text))]
public class TextInjector : MonoBehaviour
{
	[System.Serializable]
	public class Argument
	{
		public GameObject instance;
		public Component component;
		public int selectedMemberIndex;
		public string propertyPath;

		public static string P_Instance = "instance";
		public static string P_Component = "component";
		public static string P_SelectedMemberIndex = "selectedMemberIndex";
		public static string P_PropertyPath = "propertyPath";
	}

	[SerializeField, Multiline]
	string _format = "";
	[SerializeField]
	List<Argument> _arguments = new List<Argument>();

	[SerializeField]
	Text _uiText;

	void Reset()
	{
		_uiText = GetComponent<Text>();
	}

	void Awake()
	{
		_uiText = GetComponent<Text>();
	}

	object GetValue(System.Reflection.MemberInfo member, object instance)
	{
		var field = member as System.Reflection.FieldInfo;
		var prop = member as System.Reflection.PropertyInfo;
		if (field != null)
			return field.GetValue(instance);
		if (prop != null)
			return prop.GetValue(instance, null);
		return "[ERROR]";
	}

	int GetArgCount()
	{
		int cnt = 0;
		for (int i = 0; i < _format.Count(); i++)
		{
			if (_format[i] == '{')
			{
				for (int j = i+1; j < _format.Count(); j++)
				{
					if (_format[j] == '}')
					{
						++cnt;
						i = j + 1;
						break;
					}
				}
			}
		}
		return cnt;
	}

	void Resize()
	{
		int size = GetArgCount();
		if (size > _arguments.Count)
		{
			var n = size - _arguments.Count;
			for (int i = 0; i < n; i++)
				_arguments.Add(new Argument());
		}
		else if (size < _arguments.Count)
		{
			var n = _arguments.Count - size;
			for (int i = 0; i < n; i++)
				_arguments.PopBack();
		}
	}

	void TextAplly()
	{
		if(!StringUtil.CheckValidFormat(_format))
		{
			_uiText.text = "[Invalid Format]";
			return;
		}

		List<object> valueArgs = new List<object>();
		foreach (var arg in _arguments.Select((v, i) => new { v, i }))
		{
			if (arg.v.component != null)
			{
				var cmp = arg.v.component;
				var member = cmp.GetType().GetMember(arg.v.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (member.Length > 0)
				{
					object value = GetValue(member[0], cmp);
					valueArgs.Add(value);
				}
				else
					valueArgs.Add("[Null Value]");
			}
			else
				valueArgs.Add("[N/A]");
		}

		_uiText.text = string.Format(_format, valueArgs.ToArray());
	}

	void TextPreAplly()
	{
		_uiText.text = _format;
	}

	private void OnValidate()
	{
		Resize();
	}

	void Update()
	{
		TextAplly();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(TextInjector))]
	public class TextInjectorInspector : Editor
	{
		Vector2 _scrollPos = Vector2.zero;

		public override void OnInspectorGUI()
		{
			var self = target as TextInjector;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField("Text");
			var text = EditorGUILayout.TextArea(self._format, GUILayout.Height(80));

			if (!StringUtil.CheckValidFormat(self._format))
			{
				MyGUIUtil.BeginContentColorChange(Color.red);
				EditorGUILayout.LabelField("[Invalid format]");
				MyGUIUtil.EndContentColorChange();
			}

			EditorGUILayout.LabelField("Arguments");

			if (self._arguments.Count == 0)
			{
				MyGUIUtil.BeginContentColorChange(Color.yellow);
				EditorGUILayout.LabelField("[No Arguments]");
				MyGUIUtil.EndContentColorChange();
			}
			else
			{
				_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(160f));
				serializedObject.Update();
				var args = serializedObject.FindProperty("_arguments");

				for (int i = 0; i < args.arraySize; i++)
				{
					EditorGUILayout.LabelField(i.ToString() + ".");
					EditorGUILayout.BeginVertical("box");

					var arg = args.GetArrayElementAtIndex(i);
					var instance = (GameObject)arg.FindPropertyRelative(Argument.P_Instance).objectReferenceValue;
					var index = arg.FindPropertyRelative(Argument.P_SelectedMemberIndex).intValue;
					var component = (Component)arg.FindPropertyRelative(Argument.P_Component).objectReferenceValue;
					var path = arg.FindPropertyRelative(Argument.P_PropertyPath).stringValue;

					EditorGUILayout.PropertyField(arg.FindPropertyRelative(Argument.P_Instance), new GUIContent("インスタンス"));
					arg.FindPropertyRelative(Argument.P_Component).objectReferenceValue =
						MyEditorGUILayout.ComponentsPopupFromGameObject(component, instance);

					MyEditorGUILayout.FieldsPopupFromComponent(ref index, ref path, component, null, typeof(Extractable));
					arg.FindPropertyRelative(Argument.P_PropertyPath).stringValue = path;
					arg.FindPropertyRelative(Argument.P_SelectedMemberIndex).intValue = index;

					EditorGUILayout.EndVertical();
				}
				serializedObject.ApplyModifiedProperties();
				EditorGUILayout.EndScrollView();
			}

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Changed TextInjectorInspector");
				self._format = text;

				self.Resize();
				self.TextAplly();
			}

			//base.OnInspectorGUI();
		}
	}
#endif
}
