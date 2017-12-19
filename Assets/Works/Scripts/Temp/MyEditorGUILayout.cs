using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
public class Extractable : Attribute { }

#if UNITY_EDITOR
public static class MyEditorGUILayout
{
	public static Component ComponentsPopupFromGameObject(Component selected, GameObject gameObject)
	{
		if (gameObject == null)
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]GameObject is null");
			MyGUIUtil.EndContentColorChange();
			return null;
		}

		var components = gameObject.GetComponents<Component>();
		int cmpIndex = 0;
		if (selected != null)
		{
			cmpIndex = components
				.Select((item, index) => new { Index = index, Value = item })
				.Where(item => item.Value == selected)
				.Select(item => item.Index)
				.FirstOrDefault();
		}

		if (components.Count() > 0)
			cmpIndex = EditorGUILayout.Popup(cmpIndex, components.Select(item => item.GetType().Name).ToArray());
		else
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]No attached components");
			MyGUIUtil.EndContentColorChange();
			return null;
		}

		return components[cmpIndex];
	}

	/// <summary>指定のコンポーネントのフィールドを選択可能なポップアップで表示する</summary>
	public static void FieldsPopupFromComponent(ref int selectedIndex,ref object selectedField, Component component, Type filterType = null, Type filterAttr = null)
	{
		if (component == null)
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]Component is null");
			MyGUIUtil.EndContentColorChange();
			return;
		}

		var fields = component.GetType()
			.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)
			.ToArray();

		var props = component.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.ToArray();

		if (filterType != null)
		{
			fields = fields.Where(item => item.FieldType == filterType).ToArray();
			props = props.Where(item => item.PropertyType == filterType).ToArray();
		}
		if (filterAttr != null)
		{
			fields = fields.Where(item => Attribute.GetCustomAttribute(item, filterAttr) != null).ToArray();
			props = props.Where(item => Attribute.GetCustomAttribute(item, filterAttr) != null).ToArray();
		}

		var members = fields
			.Cast<MemberInfo>()
			.Union(props.Cast<MemberInfo>())
			.ToArray();

		if (members.Count() > 0)
			selectedIndex = EditorGUILayout.Popup(selectedIndex, members.Select(item => item.Name).ToArray());
		else
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]No fields matched condition");
			MyGUIUtil.EndContentColorChange();
			return;
		}

		var selectedMember = members[selectedIndex];
		var prop = selectedMember as PropertyInfo;
		var field = selectedMember as FieldInfo;

		if (prop != null)
			selectedField = prop.GetValue(component, null);
		if (field != null)
			selectedField = field.GetValue(component);
	}

	public static void FieldsPopupFromComponent(ref int selectedIndex,ref string propertyPath,Component component,Type filterType=null,Type filterAttr=null)
	{
		if (component == null)
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]Component is null");
			MyGUIUtil.EndContentColorChange();
			return;
		}

		var fields = component.GetType()
			.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.ToArray();

		var props = component.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.ToArray();

		if (filterType != null)
		{
			fields = fields.Where(item => item.FieldType == filterType).ToArray();
			props = props.Where(item => item.PropertyType == filterType).ToArray();
		}
		if (filterAttr != null)
		{
			fields = fields.Where(item => Attribute.GetCustomAttribute(item, filterAttr) != null).ToArray();
			props = props.Where(item => Attribute.GetCustomAttribute(item, filterAttr) != null).ToArray();
		}

		var members = fields
			.Cast<MemberInfo>()
			.Union(props.Cast<MemberInfo>())
			.ToArray();

		if (members.Count() > 0)
			selectedIndex = EditorGUILayout.Popup(selectedIndex, members.Select(item => item.Name).ToArray());
		else
		{
			MyGUIUtil.BeginContentColorChange(Color.yellow);
			EditorGUILayout.LabelField("[!]No fields matched condition");
			MyGUIUtil.EndContentColorChange();
			return;
		}

		var selectedMember = members[selectedIndex];
		var prop = selectedMember as PropertyInfo;
		var field = selectedMember as FieldInfo;

		if (prop != null)
			propertyPath = prop.Name;
		if (field != null)
			propertyPath = field.Name;
	}

	public static Type FieldTypesPopupFromComponent(Type selectedType, Component component)
	{
		if (component == null)
		{
			EditorGUILayout.LabelField("[!]Component is null");
			return null;
		}

		var fields = component.GetType()
			.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.ToArray();
		var props = component.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.ToArray();

		var types = fields
			.Select(item => item.FieldType)
			.Union(props.Select(item => item.PropertyType));

		var selectedIndex = types
			.Select((item, index) => new { Index = index, Value = item })
			.Where(item => item.Value == selectedType)
			.Select(item => item.Index)
			.FirstOrDefault();

		if (types.Count() > 0)
		{
			var names = types
				.Select(item => item.Name)
				.Select(name =>
				{
					if (name == "Single")
						name = "float";
					if (name == "Double")
						name = "double";
					if (name == "Boolean")
						name = "bool";
					return name;
				})
				.ToArray();
			selectedIndex = EditorGUILayout.Popup(selectedIndex, names);
		}
		else
		{
			EditorGUILayout.LabelField("[!]No fields matched condition");
			return null;
		}
		return types.ToArray()[selectedIndex];
	}
}
#endif
