using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextObj : BaseObj 
{
	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Text;

	[System.NonSerialized]
	public GameObject[] text;
	private Text textCs { get { return text[0].GetComponent<Text> (); } }

	[System.NonSerialized]
	public GameObject[] button;
	private Button buttonCs { get { return button[0].GetComponent<Button> (); } }
	
	private TextModifyMethods editMethods;
	
	override protected void AwakeAfter()
	{
		editMethods = this.gameObject.AddComponent<TextModifyMethods> ();
	}

	override protected void InitializeSetParam()
	{
		if (text == null || text.Length == 0)
		{
			text = editMethods.GetTextObj (this.gameObject);
			button = editMethods.GetButtonObj (this.gameObject);
		}
	}

	override protected void Start()
	{
		type = thisObjType;
		base.Start ();
	}
	
	override protected void LoadSaveDataMain(ObjectsData data)
	{
		textCs.text = data.texts[0];
		textCs.color = ColorClass.list[data.Colors[0]][1];

		ColorBlock colorBlock = buttonCs.colors;
		colorBlock.normalColor = ColorClass.list[data.Colors[0]][0];
		colorBlock.highlightedColor = ColorClass.list[data.Colors[0]][0];
		colorBlock.disabledColor = ColorClass.list[data.Colors[0]][0];
		colorBlock.pressedColor = ColorClass.list[data.Colors[0]][1];
		buttonCs.colors = colorBlock;
	}
	
	override protected void AddNewObjBrunch (ref ObjectsData data)
	{
		if (data == null)
		{
			Debug.Log ("data is null");
			return;
		}

		data.type = (int)thisObjType;

		data.texts = new string[1];
		data.texts[0] = textCs.text;
		data.Colors = new int[1];

		data.Colors [0] = (int)ColorClass.dic [(Color32)textCs.color];
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
				string[] tmpText = new string[1];
				tmpText[0] = textCs.text;
				item.texts = tmpText;

				int[] tmpColor = new int[1];
				tmpColor[0] = (int)ColorClass.dic [textCs.color];
				item.Colors = tmpColor;

				base.UpdateObj(item);
				return;
			}
		}
	}

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}
}
