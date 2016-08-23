using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class FrameObj : BaseObj 
{
	[System.NonSerialized]
	public Text[] text;
	[System.NonSerialized]
	public Button[] button;

	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Frame;

	public Image capcelOutline;

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}

	private Text[] GetTextObj(GameObject target)
	{
		Text[] tmpText = target.GetComponentsInChildren<Text>();
		var tmpTextList = new List<Text>();
		foreach (var item in tmpText)
		{
			if (item.gameObject.name.Contains("NameLine"))
			{
				tmpTextList.Add (item);
			}
		}
		return tmpTextList.ToArray();
	}
	private Button[] GetButtonObj(GameObject target)
	{
		Button[] tmpButton = target.GetComponentsInChildren<Button>();
		var tmpButtonList = new List<Button>();
		foreach (var item in tmpButton)
		{
			if (item.gameObject.name.Contains("Button"))
			{
				tmpButtonList.Add (item);
			}
		}
		return tmpButtonList.ToArray();
	}
	override protected void InitializeSetParam()
	{
		if (text == null || text.Length == 0)
		{
			text = GetTextObj(this.gameObject);
			button = GetButtonObj (this.gameObject);
		}
	}
	
	override protected void Start()
	{
		base.Start ();

		type = thisObjType;
	}

	override public void LoadSaveData(ObjectsData data)
	{
		base.LoadSaveData (data);

		for (int count = 0; count < text.Length; count++)
		{
			text[count].text = data.text[count];
			text[count].color = ColorClass.list[data.color[count]][1];

			ColorBlock colorBlock = button[count].colors;
			colorBlock.normalColor = ColorClass.list[data.color[count]][0];
			colorBlock.highlightedColor = ColorClass.list[data.color[count]][0];
			colorBlock.disabledColor = ColorClass.list[data.color[count]][0];
			colorBlock.pressedColor = ColorClass.list[data.color[count]][1];
			button[count].colors = colorBlock;
		}

	}

	override protected void AddObj (ObjectsData data = null)
	{
		if (data != null) {
			Debug.Log ("Exception argument in AddObj branch class");
			return;
		}

		data = new ObjectsData ();
		base.AddObj (data);

		data.type = (int)thisObjType;
		int nameLineCount = text.Length;
		data.text = new string[nameLineCount];
		data.color = new int[nameLineCount];

		for (int count = 0; count < nameLineCount; count++) {
			data.text [count] = text [count].text;
			Debug.Log ("いがああああ" + text[count].color);
			data.color [count] = (int)ColorClass.dic [text [count].color];
		}
	}

	override protected void UpdateObj (ObjectsData data = null)
	{
		if (data != null)
		{
			Debug.Log ("Exception argument in UpdateObj branch class");
			return;
		}

		foreach (var item in GeneralController.m_savedata.m_obj) 
		{
			if (item.id == this.id)
			{
				string[] tmp = new string[text.Length];
				for(int count = 0; count < text.Length; count++)
				{
					tmp[count] = text[count].text;
				}
				item.text = tmp;

				int[] tmpColor = new int[text.Length];
				for(int count = 0; count < text.Length; count++)
				{
					tmpColor[count] = (int)ColorClass.dic[ text[count].color ];
				}
				item.color = tmpColor;


				base.UpdateObj(item);
				return;
			}
		}
	}

	public void ExpandForInputLine(int lineId)
	{

	}
}
