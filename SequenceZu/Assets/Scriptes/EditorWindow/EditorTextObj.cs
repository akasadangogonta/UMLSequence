using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EditorTextObj : EditorBaseObj
{	
	TextObj baseTextObj;
	
	TextModifyMethods editMethods;
	
	override protected void StartMain () 
	{
		curEditData.editType = EditType.Button;

		editMethods = this.gameObject.AddComponent<TextModifyMethods> ();
		
		baseTextObj = (TextObj)baseObj;
		
		editTransform = this.gameObject.GetComponent<RectTransform> ();
		baseTransform = baseObj.gameObject.GetComponent<RectTransform> ();
		
		editButton = editMethods.GetButtonObj (this.gameObject);
		
		editText = editMethods.GetTextObj (this.gameObject);
		
		SetAddListener ();
		
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
		
		GetButton(baseTextObj.button).colors = colorBlock;
		GetText(baseTextObj.text).color = color32[middle];
		
		//baseObj.UpdateObjectsDataToSaveData ();
	}
	

	override protected void  ChangeColorAllowPos()
	{
		while (curEditData.id > lineNum - 1)
		{
			curEditData.id--;
			//curEditData.editType = EditType.Button;
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
			float baseY = -4;
			float oneLineDistance = 0;
			float lineNumAdjust = 1 * ((lineNum - 1) * (oneLineDistance / 2));
			float curSelectPosAdjust = -1 * (oneLineDistance * editData.id);
			
			float resultY = baseY + lineNumAdjust + curSelectPosAdjust;
			
			Vector2 leftPos = new Vector2(-220, resultY);
			Vector2 rightPos = new Vector2(220, resultY);
			
			returnVector = (isLeft == true)? leftPos : rightPos;
			break;
		}
		
		return returnVector;
	}
}