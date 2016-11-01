using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EditorFrameObj : EditorBaseObj
{
	FrameObj baseFrameObj;

	FrameModifyMethods editMethods;

	private readonly float oneLineDistance = GlobalConfig.oneLineDistanceOfFrame;

	override protected void StartMain () 
	{
		curEditData.editType = EditType.Button;

		editMethods = this.gameObject.AddComponent<FrameModifyMethods> ();

		baseFrameObj = (FrameObj)baseObj;
		
		editTransform = this.gameObject.GetComponent<RectTransform> ();
		baseTransform = baseObj.gameObject.GetComponent<RectTransform> ();
		
		editButton = editMethods.GetButtonObj (this.gameObject);

		editText = editMethods.GetTextObj (this.gameObject);

		SetAddListener ();

		lineNum = editButton.Length;

		ChangeColorAllowPos ();
	}

	public void SetAddListener()
	{
		base.SetAddListener (editButton, EditType.Button);
		base.SetAddListener (editText, EditType.Text);
	}

	override public void GetColorCallback(Color32[] color32)
	{
		switch (curEditData.editType) {
		case EditType.Text:
		case EditType.Button:
			ChangeColor (color32);
			break;
		}
	}

	public void LineEdit(EditorDirect direction, EditorAct action)
	{
		if (action == EditorAct.Add) 
		{
			CreateLineObj (ref editButton, ref baseFrameObj.button, direction, addPartsOfFrameObj[(int)PartsOfFrame.Button]);
			ChangeColorNewObj (ref editButton, ref baseFrameObj.button, EditType.Button, direction);

			CreateLineObj (ref editText, ref baseFrameObj.text, direction, addPartsOfFrameObj[(int)PartsOfFrame.LineText]);
			ChangeColorNewObj (ref editText, ref baseFrameObj.text, EditType.Text, direction);
			ChangeTextDetailNewObj (ref editText, ref baseFrameObj.text, EditType.Text, direction);

			editMethods.CreateLineObj (ref baseFrameObj.inputField, direction, baseObj.gameObject, addPartsOfFrameObj[(int)PartsOfFrame.InputField]);

			ModifyFrame (true);

			RemoveEditButtonComponents ();
			SetAddListener ();

			SetBaseRenameButton ();
			SetSiblingBaseObj ();

			ModifyLineNum(true);
			ChangeColorAllowPos ();
		} 
		else if (action == EditorAct.Remove)
		{
			DeleteLineObj (ref editButton, ref baseFrameObj.button, direction);
			
			DeleteLineObj (ref editText, ref baseFrameObj.text, direction);

			editMethods.DeleteLineObj (ref baseFrameObj.inputField, direction, baseObj.gameObject);
			
			ModifyFrame (false);

			SetAddListener ();

			SetSiblingBaseObj ();

			ModifyLineNum(false);
			ChangeColorAllowPos ();
		}
	}

	private void CreateLineObj(ref GameObject[] editTargetArray, ref GameObject[] baseTargetArray,
	                           EditorDirect direction, GameObject initData)
	{
		editMethods.CreateLineObj (ref editTargetArray, direction, this.gameObject, initData);
		editMethods.CreateLineObj (ref baseTargetArray, direction, baseObj.gameObject, initData);
	}

	private void DeleteLineObj(ref GameObject[] editTargetArray, ref GameObject[] baseTargetArray,
	                           EditorDirect direction)
	{
		editMethods.DeleteLineObj (ref editTargetArray, direction, this.gameObject);
		editMethods.DeleteLineObj (ref baseTargetArray, direction, baseObj.gameObject);
	}

	private void ChangeColor(Color32[] color32)
	{
		int thin = 0;
		int middle = 1;
		int dark = 2;
		
		System.Func<GameObject[], Button> GetButton = (target) => { return target [curEditData.id].GetComponent<Button> (); };
		System.Func<GameObject[], Text> GetText = (target) => { return target[curEditData.id].GetComponent<Text> (); };
		
		
		ColorBlock colorBlock = GetButton(editButton).colors;
		colorBlock.normalColor = color32[thin];
		colorBlock.highlightedColor = color32[thin];
		colorBlock.disabledColor = color32[thin];
		colorBlock.pressedColor = color32[middle];
		
		GetButton(editButton).colors = colorBlock;
		GetText(editText).color = color32[middle];
		
		GetButton(baseFrameObj.button).colors = colorBlock;
		GetText(baseFrameObj.text).color = color32[middle];
		
		//baseObj.UpdateObjectsDataToSaveData ();
	}
	
	private void ChangeColorNewObj(ref GameObject[] editTarget, ref GameObject[] baseTarget, 
	                               EditType editType, EditorDirect direction)
	{
		_ChangeColorNewObj (ref editTarget, editType, direction);
		_ChangeColorNewObj (ref baseTarget, editType, direction);
	}
	private void _ChangeColorNewObj(ref GameObject[] target, EditType editType, EditorDirect direction)
	{
		int oneStepBack = 1;
		int samplingNum = (direction == EditorDirect.Up) ? 0 + oneStepBack  : target.Length - 1 - oneStepBack;
		int targetNum = (direction == EditorDirect.Up) ? 0 : target.Length - 1;
		
		switch (editType)
		{
		case EditType.Text:
			Color32 copyColor = target[samplingNum].GetComponent<Text>().color;
			
			target[targetNum].GetComponent<Text>().color = copyColor;
			break;
			
		case EditType.Button:
			ColorBlock colorBlock = target[samplingNum].GetComponent<Button>().colors;
			
			target[targetNum].GetComponent<Button>().colors = colorBlock;
			break;
		}
	}
	
	private void RemoveEditButtonComponents()
	{
		foreach (var item in editButton)
		{
			System.Func<RenameButton> renameButton = () => { return item.GetComponent<RenameButton>(); };
			System.Func<ButtonSupport> buttonSupport = () => { return item.GetComponent<ButtonSupport>(); };
			if(renameButton() != null) Destroy(renameButton());
			if(buttonSupport() != null) Destroy(buttonSupport());

			System.Func<Button> button = () => { return item.GetComponent<Button>(); };
			if(button() != null) button().interactable = false;
		}
	}
	private void SetBaseRenameButton()
	{
		for (int count = 0; count < baseFrameObj.button.Length; count++) 
		{
			System.Func<RenameButton> renameButton = () => { return baseFrameObj.button[count].GetComponent<RenameButton>(); };
			if(renameButton() != null)
			{
				renameButton().renameTarget =baseFrameObj.text[count].GetComponent<Text>();
				renameButton().edit = baseFrameObj.inputField[count];
				renameButton().id = count + 1;
			}
		}
	}

	private void ChangeTextDetailNewObj(ref GameObject[] editTarget, ref GameObject[] baseTarget,
	                                    EditType editType, EditorDirect direction)
	{
		editMethods._ChangeTextDetailNewObj (ref editTarget, editType, direction);
		editMethods._ChangeTextDetailNewObj (ref baseTarget, editType, direction);
	}


	private void ModifyFrame(bool isPlus)
	{
		editMethods.ModifyFrame(editTransform, isPlus);
		editMethods.ModifyFrame(baseTransform, isPlus);
	}
	

	private void SetSiblingBaseObj()
	{
		//baseInputFieldを一番上に配置する必要がある
		editMethods.SetSibling (baseFrameObj.text);
		editMethods.SetSibling (baseFrameObj.button);
		editMethods.SetSibling (baseFrameObj.inputField);
	}

	override protected void  ChangeColorAllowPos()
	{
		while (curEditData.id > lineNum - 1)
		{
			curEditData.id--;
			curEditData.editType = EditType.Button;
			if (curEditData.id < 0) 
			{
				Debug.LogWarning("curEditData.id is minus");
				break;
			}
		}

		base.collorArrowPosCallback (_GetColorAllowPos (lineNum, curEditData, true),
		                             _GetColorAllowPos (lineNum, curEditData, false));
	}

	protected Vector2 _GetColorAllowPos(int lineNum, CurEditData editData, bool isLeft)
	{
		Vector2 returnVector = Vector2.zero;

		switch (editData.editType) 
		{
		case EditType.Button:
		case EditType.Text:
			//ColorArrowは90°傾ています。ｘとｙが逆に見える。
			float baseY = 0;
			float oneLineDistance = 31;
			float lineNumAdjust = 1 * ((lineNum - 1) * (oneLineDistance / 2));
			float curSelectPosAdjust = -1 * (oneLineDistance * editData.id);

			float resultY = baseY + lineNumAdjust + curSelectPosAdjust;

			Vector2 leftPos = new Vector2(-247, resultY);
			Vector2 rightPos = new Vector2(247, resultY);

			returnVector = (isLeft == true)? leftPos : rightPos;
			break;
		}

		return returnVector;
	}

	private void ModifyLineNum(bool isPlus)
	{
		int value = (isPlus)? 1: -1;
		lineNum += value;
		baseObj.lineNum +=  value;
	}
}