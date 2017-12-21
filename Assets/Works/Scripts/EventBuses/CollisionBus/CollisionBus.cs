using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class CollisionBus
{
	//! --------type declares--------
	public enum Timing
	{
		Enter,
		Stay,
		Exit
	}
	public struct Key
	{
		public Timing timing;
		public TagName tagA;
		public TagName tagB;

		public Key(Timing timing, TagName tagA, TagName tagB)
		{
			this.timing = timing;
			this.tagA = tagA;
			this.tagB = tagB;
		}
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("{0},{1},{2}", timing.ToString(), tagA.ToString(), tagB.ToString());
			return builder.ToString();
		}
	}
	public delegate void OnCollisionHandler(GameObject objA, GameObject objB, Collision2D collision);

	//! --------value declares--------
	static Dictionary<Key, OnCollisionHandler> _handlers = new Dictionary<Key, OnCollisionHandler>();

	//! --------functions--------
	public static Key Subscribe(Timing timing, TagName tagA, TagName tagB, OnCollisionHandler handler)
	{
		Key key = new Key(timing, tagA, tagB);
		if (_handlers.ContainsKey(key))
			_handlers[key] += handler;
		else
		{
			_handlers.Add(key, handler);
#if COL_BUS_DEBUG
			Debug.LogFormat("Key added [{0}] from [{1}]",key.ToString(),StackInfos.GetCallerFullName());
#endif
		}
		return key;
	}

	public static void Notify(Timing timing, GameObject objA, GameObject objB, Collision2D collision)
	{
		Key key = new Key(timing, objA.GetEnumTagName(), objB.GetEnumTagName());
		if (_handlers.ContainsKey(key))
		{
			_handlers[key](objA, objB, collision);
		}
		else
		{
			_handlers.Add(key, (v1, v2, v3) => { });
#if COL_BUS_DEBUG
			Debug.LogFormat("Key added [empty] from [{0}]",StackInfos.GetCallerFullName());
#endif
		}
	}

	public static void Clean()
	{
		_handlers.Clear();
	}
}
