using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


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
}