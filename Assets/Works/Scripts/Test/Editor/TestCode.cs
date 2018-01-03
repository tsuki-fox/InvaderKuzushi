using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using System.Reflection;
using System.ComponentModel;


public class TestCode
{
	public delegate void TestEventHandler();
	public event TestEventHandler testEvent = delegate { };

	[Test]
	public void CollisionBusTest()
	{
		var goA = new GameObject("objA");
		var goB = new GameObject("objB");

		CollisionBus.Notify(CollisionBus.Timing.Enter, goA, goB, null);
		CollisionBus.Subscribe(CollisionBus.Timing.Enter, TagName.Untagged, TagName.Untagged, (v1, v2, v3) => { });
	}

	public class TestClass
	{
		public delegate void EventHandler();
		public event EventHandler handler = delegate { };

		public delegate int EventHandler2(int v1);
		public event EventHandler2 handler2 = delegate { return -1; };
		
		public int value = 42;
		public const int constant = 100;

		public void Dump()
		{
			Debug.LogFormat("value:{0}", value);
			if (handler == null)
				Debug.Log("handler1 is null");
			else
				handler();
			if (handler2 == null)
				Debug.Log("handler2 is null");
			else
				handler2(0);
		}
	}

	public void InitDelegate(object obj,FieldInfo field)
	{
		field.SetValue(obj, null);
	}

	[Test]
	public void DefaultSetTest()
	{
		TestClass test = new TestClass();
		test.Dump();

		test.value = 12;
		test.handler += () => { Debug.Log("Invoked1"); };
		test.handler += () => { Debug.Log("Invoked2"); };
		test.Dump();

		var fields = test.GetType().GetFields(BindingFlags.NonPublic |
										   BindingFlags.Static |
										   BindingFlags.Instance);

		var members = test.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		foreach(var member in members)
		{
			var field = member as FieldInfo;
			if (field.IsStatic)
			{
				var raw = field.GetRawConstantValue();
				Debug.LogFormat("raw value:{0}", raw);
			}
		}

		//test.handler += () => { };
		//test.handler2 += () => { };

		test.Dump();
	}
}
/*
	FieldInfo f1 = typeof(Control).GetField("EventClick", 
				BindingFlags.Static | BindingFlags.NonPublic);
			object obj = f1.GetValue(b);
			PropertyInfo pi = b.GetType().GetProperty("Events",  
				BindingFlags.NonPublic | BindingFlags.Instance);
			EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
			list.RemoveHandler(obj, list[obj]);
*/
