using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class FrameObj : BaseObj 
{
	[System.NonSerialized]
	public GameObject[] text;
	[System.NonSerialized]
	public GameObject[] button;
	[System.NonSerialized]
	public GameObject[] inputField;
	
	public GameObject[] PartsOfFrameObj;

	private int defaultLineNum = GlobalConfig.defaultFrameLineNum;

	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Frame;

	public Image capcelOutline;

	private FrameModifyMethods editMethods;

	override protected void AwakeAfter()
	{
		editMethods = this.gameObject.AddComponent<FrameModifyMethods> ();
	}

	override protected void InitializeSetParam()
	{
		if (text == null || text.Length == 0)
		{
			text = editMethods.GetTextObj (this.gameObject);
			button = editMethods.GetButtonObj (this.gameObject);
			inputField = editMethods.GetInputObj (this.gameObject);
			base.lineNum = text.Length;
		}
	}

	override protected void Start()
	{
		base.Start ();

		type = thisObjType;
	}

	override protected void LoadSaveDataMain(ObjectsData data)
	{
		int difference = data.lineNum - defaultLineNum;

		if (difference > 0)
		{
			for (int count = 0; count < difference; count++) 
			{
				LineEdit (EditorDirect.Up, EditorAct.Add);
			}
		} 
		else if (difference < 0)
		{
			difference = Mathf.FloorToInt( Mathf.Abs((float)difference) );
			for (int count = 0; count < difference; count++) 
			{
				LineEdit (EditorDirect.Down, EditorAct.Remove);
			}
		}

		for (int count = 0; count < data.lineNum; count++)
		{
			text[count].GetComponent<Text>().text = data.texts[count];
			text[count].GetComponent<Text>().color = ColorClass.list[data.Colors[count]][1];

			ColorBlock colorBlock = button[count].GetComponent<Button>().colors;
			colorBlock.normalColor = ColorClass.list[data.Colors[count]][0];
			colorBlock.highlightedColor = ColorClass.list[data.Colors[count]][0];
			colorBlock.disabledColor = ColorClass.list[data.Colors[count]][0];
			colorBlock.pressedColor = ColorClass.list[data.Colors[count]][1];
			button[count].GetComponent<Button>().colors = colorBlock;
		}
	}

	public void LineEdit(EditorDirect direction, EditorAct action)
	{
		if (action == EditorAct.Add) 
		{
			editMethods.CreateLineObj(ref button, direction, this.gameObject, PartsOfFrameObj[(int)PartsOfFrame.Button]);
			editMethods.CreateLineObj(ref text, direction, this.gameObject, PartsOfFrameObj[(int)PartsOfFrame.LineText]);
			editMethods.CreateLineObj (ref inputField, direction,this.gameObject, PartsOfFrameObj[(int)PartsOfFrame.InputField]);
			
			editMethods.ModifyFrame (this.gameObject.GetComponent<RectTransform> (), isPlus:true);
			
			SetBaseRenameButton ();
			SetSiblingBaseObj ();
		} 
		else if (action == EditorAct.Remove)
		{
			editMethods.DeleteLineObj(ref button, direction, this.gameObject);
			editMethods.DeleteLineObj(ref text, direction, this.gameObject);
			editMethods.DeleteLineObj (ref inputField, direction,this.gameObject);
			
			editMethods.ModifyFrame (this.gameObject.GetComponent<RectTransform> (), isPlus:false);

			SetSiblingBaseObj ();
		}
	}

	private void SetBaseRenameButton()
	{
		for (int count = 0; count < button.Length; count++) 
		{
			System.Func<RenameButton> renameButton = () => { return button[count].GetComponent<RenameButton>(); };
			if(renameButton() != null)
			{
				renameButton().renameTarget = text[count].GetComponent<Text>();
				renameButton().edit = inputField[count];
				renameButton().id = count + 1;
			}
		}
	}
	private void SetSiblingBaseObj()
	{
		//baseInputFieldを一番上に配置する必要がある
		editMethods.SetSibling (text);
		editMethods.SetSibling (button);
		editMethods.SetSibling (inputField);
	}


	override protected void AddNewObjBrunch (ref ObjectsData data)
	{
		if (data == null) 
		{
			Debug.Log ("data is null");
			return;
		}

		data.type = (int)thisObjType;
		lineNum = text.Length;
		data.lineNum = lineNum;
		data.texts = new string[lineNum];
		data.Colors = new int[lineNum];


		for (int count = 0; count < lineNum; count++)
		{
			data.texts [count] = text [count].GetComponent<Text>().text;
			data.Colors [count] = (int)ColorClass.dic 
				                 [text [count].GetComponent<Text>().color];
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
					tmp[count] = text[count].GetComponent<Text>().text;
				}
				item.texts = tmp;

				int[] tmpColor = new int[text.Length];
				for(int count = 0; count < text.Length; count++)
				{
					tmpColor[count] = (int)ColorClass.dic
						              [ text[count].GetComponent<Text>().color ];
				}
				item.Colors = tmpColor;

				item.lineNum = tmp.Length;

				base.UpdateObj(item);
				return;
			}
		}
	}

	public void ExpandForInputLine(int lineId)
	{

	}

	public void InitializeModifyFrame()
	{

	}

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}
}
