using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class TagNameConstructor
{
	static readonly string[] INVALID_CHARS =
	{
		  " ", "!", "\"", "#", "$",
		"%", "&", "\'", "(", ")",
		"-", "=", "^",  "~", "\\",
		"|", "[", "{",  "@", "`",
		"]", "}", ":",  "*", ";",
		"+", "/", "?",  ".", ">",
		",", "<"
	};

	const string ITEM_NAME = "Tools/Create/Tag Name";
	const string PATH = "Assets/Works/Scripts/Temp/TagName.cs";

	static readonly string FILENAME = Path.GetFileName(PATH);
	static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);
	
	[MenuItem(ITEM_NAME)]
	public static void Create()
	{
		if (!CanCreate())
			return;
		CreateScript();
		EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
	}

	public static void CreateScript()
	{
		var builder = new StringBuilder();
		builder.AppendFormat("public enum {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
		builder.AppendLine("{");
		foreach (var n in InternalEditorUtility.tags.Select(c => new { var = RemoveInvalidChars(c), val = c }))
			builder.Append("\t").AppendFormat(@"{0},", n.var).AppendLine();
		builder.AppendLine("}");

		var directoryName = Path.GetDirectoryName(PATH);
		if (!Directory.Exists(directoryName))
			Directory.CreateDirectory(directoryName);

		File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

	}

	[MenuItem(ITEM_NAME,true)]
	public static bool CanCreate()
	{
		return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
	}

	public static string RemoveInvalidChars(string str)
	{
		Array.ForEach(INVALID_CHARS, c => str = str.Replace(c, string.Empty));
		return str;
	}

}