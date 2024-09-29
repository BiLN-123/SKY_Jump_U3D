using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App 
{
	public static void Trace(object t, DebugColor color = DebugColor.normal)
	{
		if (color != DebugColor.normal) 
		{
			Debug.Log("<color=" + color.ToString() + ">" + t.ToString() + "</color>");
		}
		else 
		{
			Debug.Log(t.ToString());	
		}
	}
}
 public enum DebugColor
 {
 	normal, white, yellow, red, green
 }
