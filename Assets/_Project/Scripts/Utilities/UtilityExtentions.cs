using System.Collections.Generic;
using UnityEngine;

public static class UtilityExtentions
{
	public static void Shuffle<T>(this IList<T> ts)
	{
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i)
		{
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}

	public static string ToHex(this Color color)
	{
		return $"#{ColorUtility.ToHtmlStringRGB(color)}";
	}
}