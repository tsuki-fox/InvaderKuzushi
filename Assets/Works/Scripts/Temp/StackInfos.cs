using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class StackInfos
{
	public static string GetCallerFullName()
	{
		var caller = new StackFrame(2);
		string methodName = caller.GetMethod().Name;
		string className = caller.GetMethod().ReflectedType.FullName;
		return className + "." + methodName;
	}
}