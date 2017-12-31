using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformEx
{
	#region pos,angles
	public static void SetPositionX(this Transform src,float x)
	{
		src.position = new Vector3(x, src.position.y, src.position.z);
	}
	public static void SetPositionY(this Transform src, float y)
	{
		src.position = new Vector3(src.position.x, y, src.position.z);
	}
	public static void SetPositionZ(this Transform src,float z)
	{
		src.position = new Vector3(src.position.x, src.position.y, z);
	}
	public static void SetEulerAnglesX(this Transform src,float x)
	{
		src.eulerAngles = new Vector3(x, src.eulerAngles.y, src.eulerAngles.z);
	}
	public static void SetEulerAnglesY(this Transform src,float y)
	{
		src.eulerAngles = new Vector3(src.eulerAngles.x, y, src.eulerAngles.z);
	}
	public static void SetEulerAnglesZ(this Transform src,float z)
	{
		src.eulerAngles = new Vector3(src.eulerAngles.x, src.eulerAngles.y, z);
	}
	public static void AddPositionX(this Transform src,float x)
	{
		src.position += new Vector3(x, 0f, 0f);
	}
	public static void AddPositionY(this Transform src,float y)
	{
		src.position += new Vector3(0f, y, 0f);
	}
	public static void AddPositionZ(this Transform src, float z)
	{
		src.position += new Vector3(0f, 0f, z);
	}
	public static void AddEulerAnglesX(this Transform src,float x)
	{
		src.eulerAngles += new Vector3(x, 0f, 0f);
	}
	public static void AddEulerAnglesY(this Transform src,float y)
	{
		src.eulerAngles += new Vector3(0f, y, 0f);
	}
	public static void AddEulerAnglesZ(this Transform src,float z)
	{
		src.eulerAngles += new Vector3(0f, 0f, z);
	}
	#endregion
}
