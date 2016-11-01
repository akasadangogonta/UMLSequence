using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorTriangleObj : EditorBaseObj
{
	TriangleObj baseTriangleObj;
	
	TriangleModifyMethods editMethods;
	
	override protected void StartMain () 
	{
		curEditData.editType = EditType.Button;

		editMethods = this.gameObject.AddComponent<TriangleModifyMethods> ();
		
		baseTriangleObj = (TriangleObj)baseObj;
		
		editTransform = this.gameObject.GetComponent<RectTransform> ();
		baseTransform = baseObj.gameObject.GetComponent<RectTransform> ();
		
		editButton = editMethods.GetButtonObj (this.gameObject);
		
		SetAddListener ();
		
		lineNum = editButton.Length;
		
		ChangeColorAllowPos ();
	}
	
	public void SetAddListener()
	{
		base.SetAddListener (editButton, EditType.Button);;
	}
	
	override public void GetColorCallback(Color32[] color32)
	{
		switch (curEditData.editType)
		{
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
		
		System.Func<GameObject[], Button> GetButton = (target) => { 
			return target [curEditData.id].GetComponent<Button> (); 
		};
		
		
		ColorBlock colorBlock = GetButton(editButton).colors;
		colorBlock.normalColor = color32[thin];
		colorBlock.highlightedColor = color32[thin];
		colorBlock.disabledColor = color32[thin];
		colorBlock.pressedColor = color32[middle];
		
		GetButton(editButton).colors = colorBlock;
		
		GetButton(baseTriangleObj.button).colors = colorBlock;
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
