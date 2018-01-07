using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UniRx;

public static class IEnumerableEx
{
	public static TSource FindMin<TSource,TResult>(this IEnumerable<TSource> self,Func<TSource,TResult> selector)
	{
		return self.FirstOrDefault(c => selector(c).Equals(self.Min(selector)));
	}
}