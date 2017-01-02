using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorTriangleObj : EditorBaseObj
{
	TriangleObj baseTriangleObj;
	
	TriangleModifyMethods editMethods;
	
	override protected void StartMain () 
	{
		curEditData.editType = EditType.Image;

		editMethods = this.gameObject.AddComponent<TriangleModifyMethods> ();
		
		baseTriangleObj = (TriangleObj)baseObj;
		
		editTransform = this.gameObject.GetComponent<RectTransform> ();
		baseTransform = baseObj.gameObject.GetComponent<RectTransform> ();

		editImage = editMethods.GetImageObj(this.gameObject);

		SetAddListener ();
		
		ChangeColorAllowPos ();
	}
	
	public void SetAddListener()
	{
		base.SetAddListener (editImage, EditType.Image);
	}
	
	override public void GetColorCallback(Color32[] color32)
	{
		Debug.Log ("GetColorCallback Invoke");

		switch (curEditData.editType)
		{
		case EditType.Image:
			ChangeColor (color32);
			break;
		default:
			Debug.LogWarning ("NotFound EditType " + curEditData.editType);
			break;
		}
	}
	
	private void ChangeColor(Color32[] color32)
	{
		Debug.Log ("ChangeColor Invoke");

		int thin = 0;
		int middle = 1;
		int dark = 2;


		System.Func<GameObject[], Image> GetImage = (target) => { return target [curEditData.id].GetComponent<Image> (); };

		GetImage(editImage).color = color32[thin];
		GetImage(baseTriangleObj.image).color = color32[thin];
	}
	
	override protected void  ChangeColorAllowPos()
	{
		/*
		base.collorArrowPosCallback (_GetColorAllowPos (lineNum, curEditData, true),
		                             _GetColorAllowPos (lineNum, curEditData, false));
		                             */
	}
	/*
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
	*/
}
