using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class StringUtil
{
	/// <summary>有効なフォーマットかどうか</summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public static bool CheckValidFormat(string text)
	{
		const int Proc = 0;
		const int Ready = 1;

		int state = Ready;
		string num = "";
		int prev = -1;
		foreach (var c in text)
		{
			if (state == Ready)
			{
				if (c == '{')
					state = Proc;
				else if (c == '}')
					return false;
			}
			else if (state == Proc)
			{
				if (c == '{')
					return false;
				if (c == '}')
				{
					int v = 0;
					if (num == "")
						return false;
					if (!int.TryParse(num, out v))
						return false;
					if (v != prev + 1)
						return false;
					prev = v;
					state = Ready;
					num = "";
				}
				else
					num += c;
			}
		}

		if (state == Proc)
			return false;

		return true;
	}

	public static string GetPathWithoutExtension(string path)
	{
		var ext = Path.GetExtension(path);
		if (string.IsNullOrEmpty(ext))
			return path;
		return path.Replace(ext, string.Empty);
	}
}
