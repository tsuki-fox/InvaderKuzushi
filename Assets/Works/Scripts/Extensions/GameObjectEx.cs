using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameObjectEx
{
	public static void SetChildrenLayer(this GameObject self, int layer, bool containSelf = true)
	{
		if (containSelf)
			self.layer = layer;
		foreach (Transform item in self.transform)
			item.gameObject.layer = layer;
	}

	public static void SetChildrenTag(this GameObject self,TagName tag,bool containSelf=true)
	{
		if (containSelf)
			self.tag = tag.ToString();
		foreach (Transform item in self.transform)
			item.gameObject.tag = tag.ToString();
	}

	public static TagName GetEnumTagName(this GameObject self)
	{
		return (TagName)Enum.Parse(typeof(TagName), self.tag);
	}

	public static bool HasComponent<T>(this GameObject self)where T:Component
	{
		return self.GetComponent<T>() != null;
	}

	public static bool IsChild(this GameObject self,GameObject obj)
	{
		var current = obj.transform;
		while(current!=self)
		{
			if (current.parent == null)
				return false;
			current = current.parent;
			if (current == self)
				return true;
		}
		return false;
	}
}