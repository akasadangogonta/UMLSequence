using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PartsIndex
{
	button = 0,
	line = 1,
	input = 2,
}

public class AddParts
{
	public List<GameObject> parts = new List<GameObject> ();
}

public class EditorWindowControll : MonoBehaviour {
	private readonly float oneLineDistance = 33.0F;

	public ModifyLineButton[] newLineButton;
	public ModifyLineButton[] delLineButton;

	public ColorButton[] colorButton;
	public GameObject[] colorArrowIcon;

	private BaseObj originObj;
	private EditorTargetControllBase targetObj;

	//instantiateParts
	public GameObject partsButton;//0
	public GameObject partsNewLine;//1
	public GameObject partsInputField;//2
	private AddParts addParts;

	void Start()
	{
		addParts = new AddParts ();
		addParts.parts.Add (partsButton); //0
		addParts.parts.Add (partsNewLine); //1
		addParts.parts.Add (partsInputField); //2
	}

	public void SetData(BaseObj originObj, EditorTargetControllBase targetObj)
	{
		this.targetObj = targetObj;
		targetObj.SetData (originObj, addParts);
		targetObj.SetCallback (CollorArrowPointerChangePos);

		foreach (var item in colorButton)
		{
			item.SendColorCallback = targetObj.GetColorCallback;
		}

		foreach (var item in newLineButton) 
		{
			item.SetData(targetObj, LineEditButtonChangePos);
		}
		foreach (var item in delLineButton) 
		{
			item.SetData(targetObj, LineEditButtonChangePos);
		}
	}

	private void LineEditButtonChangePos(bool isLineIncreace)
	{
		System.Func<int, Transform> NewBtnTrans = (index) => { return newLineButton [index].transform; };
		System.Func<int, Transform> DelBtnTrans = (index) => { return delLineButton [index].transform; };

		int sign = (isLineIncreace == true)? 1: -1;

		if (newLineButton.Length == 2)
		{
			NewBtnTrans(0).localPosition = new Vector2(NewBtnTrans(0).localPosition.x,
			                                           NewBtnTrans(0).localPosition.y + (sign * oneLineDistance / 2));
			NewBtnTrans(1).localPosition = new Vector2(NewBtnTrans(1).localPosition.x,
			                                           NewBtnTrans(1).localPosition.y - (sign * oneLineDistance / 2));
		}
		if (delLineButton.Length == 2)
		{
			DelBtnTrans(0).localPosition = new Vector2(DelBtnTrans(0).localPosition.x,
			                                           DelBtnTrans(0).localPosition.y + (sign * oneLineDistance / 2));
			DelBtnTrans(1).localPosition = new Vector2(DelBtnTrans(1).localPosition.x,
			                                           DelBtnTrans(1).localPosition.y - (sign * oneLineDistance / 2));
		}
	}

	private void CollorArrowPointerChangePos(Vector2 leftPos, Vector2 rightPos)
	{
		colorArrowIcon [0].transform.localPosition = leftPos;
		colorArrowIcon [1].transform.localPosition = rightPos;

	}
}
