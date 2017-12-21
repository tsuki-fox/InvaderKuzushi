using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LifeCycleBus
{
	//! --------type declares--------
	public delegate void OnFree(GameObject self);

	//! --------value declares--------
	static Dictionary<TagName, OnFree> _handlers = new Dictionary<TagName, OnFree>();

	//! --------functions--------
	public static void Subscribe(TagName tag,OnFree handler)
	{
		if (_handlers.ContainsKey(tag))
			_handlers[tag] += handler;
		else
			_handlers.Add(tag, handler);
	}

	public static void Notify(GameObject self)
	{
		var tag = self.GetEnumTagName();

		if (_handlers.ContainsKey(tag))
			_handlers[tag](self);
		else
			_handlers.Add(tag, _ => { });
	}

	public static void Clean()
	{
		_handlers = new Dictionary<TagName, OnFree>();
	}
}
