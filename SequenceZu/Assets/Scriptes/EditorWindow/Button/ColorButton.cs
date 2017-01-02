using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ColorType
{
	Black = 0,
	Red,
	Green,
	Blue,
	Gray,
	Purple,
	Yellow,
	Water,
}

public enum ColorDepth
{
	thin = 0,
	middle,
	dark,
}

public struct Colors
{
	static public Color32[] Black =  {new Color32(159, 159, 159, 255),
		new Color32(45, 45, 45, 255)};
	static public Color32[] Red =    {new Color32(245, 137, 137, 255),
		new Color32(187, 0, 0, 255)};
	static public Color32[] Green =  {new Color32(130, 208, 140, 255),
		new Color32(0, 140, 0, 255)};
	static public Color32[] Blue =   {new Color32(136, 130, 208, 255),
		new Color32(60, 38, 176, 255)};
	
	static public Color32[] Gray =   {new Color32(223, 223, 223, 255),
		new Color32(150, 150, 150, 255)};
	static public Color32[] Purple = {new Color32(238, 180, 232, 255),
		new Color32(162, 33, 182, 255)};
	static public Color32[] Yellow = {new Color32(238, 242, 178, 255),
		new Color32(126, 128, 0, 255)};
	static public Color32[] Water =  {new Color32(187, 221, 218, 255),
		new Color32(48, 171, 176, 255)};
}

public static class ColorClass
{
	public static Dictionary<Color, ColorType> dic = new Dictionary<Color, ColorType>();
	public static List<Color32[]> list = new List<Color32[]> ();

	public static void Initialize()
	{
		//var list = new List<Color32[]> ();
		list.Add (Colors.Black);
		list.Add (Colors.Red);
		list.Add (Colors.Green);
		list.Add (Colors.Blue);
		list.Add (Colors.Gray);
		list.Add (Colors.Purple);
		list.Add (Colors.Yellow);
		list.Add (Colors.Water);

		int length = System.Enum.GetNames (typeof(ColorType)).Length;

		for (int i = 0; i < length; i++) 
		{
			foreach (var item in list[i])
			{
				//全てColorを一つずつdicのキーとして格納　>> Black[0] >> Black[1] >> Red[0] など
				//バリューにはそれがどの色タイプかを格納、Black[0]とBlack[1]はブラックタイプということになる
				Debug.Log ("color[" + i + "] = " + item + " = " + (Color)item);
				dic.Add ((Color)item, (ColorType)i);
			}
		}
	}
}

public class ColorButton : AutoAddEventTrigger
{
	public ColorType colorType;

	[System.NonSerialized]
	public System.Action<Color32[]> SendColorCallback;

	override public void OnMouseDown()
	{
		List<Color32> color32 = new List<Color32>();

		switch (colorType)
		{
		case ColorType.Black:
			color32.AddRange(Colors.Black);
			break;
		case ColorType.Red:
			color32.AddRange(Colors.Red);
			break;
		case ColorType.Green:
			color32.AddRange(Colors.Green);
			break;		
		case ColorType.Blue:
			color32.AddRange(Colors.Blue);
			break;

		case ColorType.Gray:
			color32.AddRange(Colors.Gray);
			break;
		case ColorType.Purple:
			color32.AddRange(Colors.Purple);
			break;
		case ColorType.Yellow:
			color32.AddRange(Colors.Yellow);
			break;
		case ColorType.Water:
			color32.AddRange(Colors.Water);
			break;
		}
		SendColorCallback (color32.ToArray());
	}
}
