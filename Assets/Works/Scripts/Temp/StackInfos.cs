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

	public static string GetCallerTrace(int depth)
	{
		string tracer = "";
		for (int f1 = 2; f1 < depth + 2; f1++)
		{
			tracer += ">";
			var caller = new StackFrame(f1);
			string method = caller.GetMethod().Name;
			string className = caller.GetMethod().ReflectedType.FullName;
			tracer += className + "." + method;
		}
		return tracer;
	}
}