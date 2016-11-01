using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextModifyMethods : ModifyMethodsBase
{
	private readonly float oneLineDistance = GlobalConfig.oneLineDistanceOfText;

	
	public void ModifyTextArea(RectTransform target, bool isPlus)
	{
		int sign = (isPlus == true) ? 1 : -1;
		target.sizeDelta = new Vector2 (target.sizeDelta.x, target.sizeDelta.y + (oneLineDistance * sign));
	}


	public void ObjArrayControll(ref GameObject[] targetArray,  EditorDirect direction, EditorAct action, 
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
	

	public void RenameGameobjectName(ref GameObject[] editTarget, EditorDirect direction)
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


	public void SetSibling(GameObject[] targetObj)
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
		
		SetSibling (targetObj);
	}

	public void _ChangeTextDetailNewObj(ref GameObject[] target, EditType editType, EditorDirect direction)
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
}
