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

	//instantiateParts
	public GameObject[] PartsOfFrameObj;
	
	private EditFrameMethods editMethods = new EditFrameMethods ();

	private int defaultLineNum = GlobalConfig.defaultFrameLineNum;

	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Frame;

	public Image capcelOutline;

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}

	override protected void InitializeSetParam()
	{
		if (text == null || text.Length == 0)
		{
			text = editMethods.GetTextObj(this.gameObject);
			button = editMethods.GetButtonObj (this.gameObject);
			inputField = editMethods.GetInputObj (this.gameObject);
			//base.lineNum = text.Length;
		}
	}
	
	override protected void Start()
	{
		base.Start ();

		type = thisObjType;

		//lineNum = defaultLineNum;
	}

	override protected void LoadSaveDataMain(ObjectsData data)
	{
		//base.LoadSaveData (data);

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
			text[count].GetComponent<Text>().text = data.text[count];
			text[count].GetComponent<Text>().color = ColorClass.list[data.color[count]][1];

			ColorBlock colorBlock = button[count].GetComponent<Button>().colors;
			colorBlock.normalColor = ColorClass.list[data.color[count]][0];
			colorBlock.highlightedColor = ColorClass.list[data.color[count]][0];
			colorBlock.disabledColor = ColorClass.list[data.color[count]][0];
			colorBlock.pressedColor = ColorClass.list[data.color[count]][1];
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
			/*
			DeleteLineObj (ref editButton, ref baseFrameObj.button, direction);
			
			DeleteLineObj (ref editText, ref baseFrameObj.text, direction);
			
			editMethods.DeleteLineObj (ref baseFrameObj.inputField, direction, baseObj.gameObject);
			
			ModifyFrame (false);
			
			SetAddListener ();
			
			SetSiblingBaseObj ();
			
			ModifyLineNum(false);
			ChangeColorAllowPos ();
			*/
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


	override protected void AddNewObj (ObjectsData data = null)
	{
		if (data != null) 
		{
			Debug.Log ("Exception argument in AddObj branch class");
			return;
		}

		data = new ObjectsData ();
		base.AddNewObj (data);

		data.type = (int)thisObjType;
		lineNum = text.Length;
		data.lineNum = lineNum;
		data.text = new string[lineNum];
		data.color = new int[lineNum];


		for (int count = 0; count < lineNum; count++)
		{
			data.text [count] = text [count].GetComponent<Text>().text;
			data.color [count] = (int)ColorClass.dic 
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
				item.text = tmp;

				int[] tmpColor = new int[text.Length];
				for(int count = 0; count < text.Length; count++)
				{
					tmpColor[count] = (int)ColorClass.dic
						              [ text[count].GetComponent<Text>().color ];
				}
				item.color = tmpColor;

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
}
