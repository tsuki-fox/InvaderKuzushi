using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ListEx
{
	public static void RemoveAtList<T>(this List<T> self,List<T> list)
	{
		foreach (var item in list)
			self.Remove(item);
	}

	public static T PopBack<T>(this List<T> self)
	{
		var result = self.Last();
		self.Remove(result);
		return result;
	}
}
