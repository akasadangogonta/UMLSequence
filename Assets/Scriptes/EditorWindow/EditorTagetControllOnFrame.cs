using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EditorTagetControllOnFrame : EditorTargetControllBase
{
	//[System.NonSerialized]
	public RectTransform editFrameTransform;
	public RectTransform baseFrameTransform;

	public GameObject[] editText;
	public GameObject[] editButton;
	//public GameObject[] editImage;
	
	public GameObject[] baseText;
	public GameObject[] baseButton;
	//public GameObject[] baseImage;

	public GameObject[] baseInputField;


	private readonly float oneLineDistance = 33.0F;

	override protected void Start () 
	{
		base.Start ();
		
		editFrameTransform = this.gameObject.GetComponent<RectTransform> ();
		baseFrameTransform = baseObj.gameObject.GetComponent<RectTransform> ();
		
		editButton = GetButtonObj (this.gameObject);
		baseButton = GetButtonObj (baseObj.gameObject);
		
		editText = GetTextObj (this.gameObject);
		baseText = GetTextObj (baseObj.gameObject);

		baseInputField = GetInputObj (baseObj.gameObject);
		
		SetAddListener ();

		lineNum = editButton.Length;

		ChangeColorAllowPos ();
	}

	public void SetAddListener()
	{
		base.SetAddListener (editButton, EditType.Button);
		base.SetAddListener (editText, EditType.Text);
	}

	private GameObject[] GetTextObj(GameObject target)
	{
		Text[] tmpText = target.GetComponentsInChildren<Text>();
		var tmpTextList = new List<GameObject>();
		foreach (var item in tmpText)
		{
			if (item.gameObject.name.Contains("NameLine"))
			{
				tmpTextList.Add (item.gameObject);
			}
		}
		return tmpTextList.ToArray();
	}
	private GameObject[] GetButtonObj(GameObject target)
	{
		Button[] tmpButton = target.GetComponentsInChildren<Button>();
		var tmpButtonList = new List<GameObject>();
		foreach (var item in tmpButton)
		{
			if (item.gameObject.name.Contains("Button"))
			{
				tmpButtonList.Add (item.gameObject);
			}
		}
		return tmpButtonList.ToArray();
	}
	private GameObject[] GetInputObj(GameObject target)
	{
		InputField[] tmpInput = target.GetComponentsInChildren<InputField>();
		var tmpInputList = new List<GameObject>();
		foreach (var item in tmpInput)
		{
			if (item.gameObject.name.Contains("InputField"))
			{
				tmpInputList.Add (item.gameObject);
			}
		}
		return tmpInputList.ToArray();
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
			CreateLineObj (ref editButton, ref baseButton, direction, addParts.parts [(int)PartsIndex.button]);
			ChangeColorNewObj (ref editButton, ref baseButton, EditType.Button, direction);

			CreateLineObj (ref editText, ref baseText, direction, addParts.parts [(int)PartsIndex.line]);
			ChangeColorNewObj (ref editText, ref baseText, EditType.Text, direction);
			ChangeTextDetailNewObj (ref editText, ref baseText, EditType.Text, direction);

			CreateLineObj (ref baseInputField, direction, baseObj.gameObject, addParts.parts [(int)PartsIndex.input]);

			ModifyFrame (true);

			RemoveEditButtonComponents ();
			SetAddListener ();

			SetBaseRenameButton ();
			SetSiblingBaseObj ();

			lineNum++;
			ChangeColorAllowPos ();
		} 
		else if (action == EditorAct.Remove)
		{
			DeleteLineObj (ref editButton, ref baseButton, direction);
			
			DeleteLineObj (ref editText, ref baseText, direction);

			DeleteLineObj (ref baseInputField, direction, baseObj.gameObject);
			
			ModifyFrame (false);

			SetAddListener ();

			SetSiblingBaseObj ();

			lineNum--;
			ChangeColorAllowPos ();
		}
	}

	private void CreateLineObj(ref GameObject[] editTargetArray, ref GameObject[] baseTargetArray,
	                           EditorDirect direction, GameObject initData)
	{
		CreateLineObj (ref editTargetArray, direction, this.gameObject, initData);
		CreateLineObj (ref baseTargetArray, direction, baseObj.gameObject, initData);
	}
	private void CreateLineObj(ref GameObject[] targetObj, EditorDirect direction, GameObject parentObj,
	                           GameObject initData)
	{
		GameObject newObj = Instantiate (initData) as GameObject;
		newObj.transform.parent = parentObj.transform;
		//SetSibling (newObj);
		newObj.transform.localScale = new Vector3 (1, 1, 1);

		ObjArrayControll (ref targetObj, direction, EditorAct.Add, newObj);

		AdjustPositionObj(ref targetObj, direction, EditorAct.Add, newObj);	
		
		RenameGameobjectName (ref targetObj, direction);
	}

	private void DeleteLineObj(ref GameObject[] editTargetArray, ref GameObject[] baseTargetArray,
	                           EditorDirect direction)
	{
		DeleteLineObj (ref editTargetArray, direction, this.gameObject);
		DeleteLineObj (ref baseTargetArray, direction, baseObj.gameObject);
	}
	private void DeleteLineObj(ref GameObject[] targetObj, EditorDirect direction, GameObject parentObj)
	{
		int index = (direction == EditorDirect.Up) ? 0 : targetObj.Length - 1;

		Destroy (targetObj [index]);
		
		ObjArrayControll (ref targetObj, direction, EditorAct.Remove);
		
		AdjustPositionObj(ref targetObj, direction, EditorAct.Remove);

		RenameGameobjectName (ref targetObj, direction);
	}

	private void AdjustPositionObj(ref GameObject[] editTarget, EditorDirect direction, EditorAct editorAct,
	                               GameObject newObj = null)
	{
		if (editorAct == EditorAct.Add)
		{
			int oneStepBack = 1;
			int samplingNum = (direction == EditorDirect.Up) ? 0 + oneStepBack : editTarget.Length - 1 - oneStepBack;

			Vector2 vec = editTarget [samplingNum].transform.localPosition;
			vec = new Vector2 (vec.x, vec.y + (oneLineDistance * (int)direction));
			newObj.transform.localPosition = vec;

			float adjust = (direction == EditorDirect.Up) ? -1 * (oneLineDistance / 2) : 1 * oneLineDistance / 2;
			foreach (var item in editTarget)
			{
				item.transform.localPosition = new Vector3 (item.transform.localPosition.x,
				                                            item.transform.localPosition.y + adjust,
				                                            item.transform.localPosition.z);
			}
		} 
		else if (editorAct == EditorAct.Remove)
		{
			float adjust = (direction == EditorDirect.Up) ? 1 * (oneLineDistance / 2) : -1 * oneLineDistance / 2;
			foreach (var item in editTarget) 
			{
				item.transform.localPosition = new Vector3 (item.transform.localPosition.x,
				                                            item.transform.localPosition.y + adjust,
				                                            item.transform.localPosition.z);
			}
		}
	}
	private void RenameGameobjectName(ref GameObject[] editTarget, EditorDirect direction)
	{
		string[] objName = editTarget [0].name.Split('_');
		if (objName [0].Contains ("(Clone)") == true) 
		{
			objName[0] = objName[0].Replace ("(Clone)", "");
		}

		int num = 1;
		foreach (var item in editTarget)
		{
			item.name = objName[0] + "_" + num;
			num++;
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
		
		GetButton(baseButton).colors = colorBlock;
		GetText(baseText).color = color32[middle];
		
		baseObj.UpdateObjectsDataToSaveData ();
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
		for (int count = 0; count < baseButton.Length; count++) 
		{
			System.Func<RenameButton> renameButton = () => { return baseButton[count].GetComponent<RenameButton>(); };
			if(renameButton() != null)
			{
				renameButton().renameTarget = baseText[count].GetComponent<Text>();
				renameButton().edit = baseInputField[count];
				renameButton().id = count + 1;
			}
		}
	}

	private void ChangeTextDetailNewObj(ref GameObject[] editTarget, ref GameObject[] baseTarget,
	                                    EditType editType, EditorDirect direction)
	{
		_ChangeTextDetailNewObj (ref editTarget, editType, direction);
		_ChangeTextDetailNewObj (ref baseTarget, editType, direction);
	}
	private void _ChangeTextDetailNewObj(ref GameObject[] target, EditType editType, EditorDirect direction)
	{
		int oneStepBack = 1;
		int samplingNum = (direction == EditorDirect.Up) ? 0 + oneStepBack  : target.Length - 1 - oneStepBack;
		int targetNum = (direction == EditorDirect.Up) ? 0 : target.Length - 1;
		
		switch (editType)
		{
		case EditType.Text:
			string str = target[samplingNum].GetComponent<Text>().text;
			
			target[targetNum].GetComponent<Text>().text = str;// + "_Add";
			break;
		}
	}

	private void ModifyFrame(bool isPlus)
	{
		_ModifyFrame(editFrameTransform, isPlus);
		_ModifyFrame(baseFrameTransform, isPlus);
	}
	private void _ModifyFrame(RectTransform target, bool isPlus)
	{
		int sign = (isPlus == true) ? 1 : -1;
		target.sizeDelta = new Vector2 (target.sizeDelta.x, target.sizeDelta.y + (oneLineDistance * sign));
	}


	private void ObjArrayControll(ref GameObject[] targetArray,  EditorDirect direction, EditorAct action, 
	                               GameObject newObj = null)
	{
		var tmpList = new List<GameObject>();

		if (action == EditorAct.Add)
		{
			tmpList.AddRange (targetArray);
			if (direction == EditorDirect.Up)
			{
				tmpList.Insert(0, newObj);
			}else
			{
				tmpList.Add(newObj);
			}
		} 
		else if (action == EditorAct.Remove)
		{
			tmpList.AddRange (targetArray);
			if (direction == EditorDirect.Up)
			{
				tmpList.RemoveAt(0);
			}else
			{
				tmpList.RemoveAt(tmpList.Count - 1);
			}
		}

		targetArray = tmpList.ToArray ();
	}

	private void SetSiblingBaseObj()
	{
		int siblingCount = 0;

		System.Action<GameObject[]> SetSibling = (target) =>
		{
			foreach (var item in target) 
			{
				item.transform.SetSiblingIndex(siblingCount);
				siblingCount++;
			}
		};

		SetSibling (baseInputField);
		SetSibling (baseButton);
		SetSibling (baseText);
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
			float baseX = 0;
			float oneLineDistance = 31;
			float lineNumAdjust = -1 * ((lineNum - 1) * (oneLineDistance / 2));
			float curSelectPosAdjust = (oneLineDistance * editData.id);

			float resultX = baseX + lineNumAdjust + curSelectPosAdjust;

			Vector2 leftPos = new Vector2(resultX, -247);
			Vector2 rightPos = new Vector2(resultX, 247);

			returnVector = (isLeft == true)? leftPos : rightPos;
			break;
		}

		return returnVector;
	}
}