using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FrameModifyMethods : ModifyMethodsBase
{
	private readonly float oneLineDistance = GlobalConfig.oneLineDistanceOfFrame;
	
	public void CreateLineObj(ref GameObject[] targetObj, EditorDirect direction, GameObject parentObj,
	                           GameObject initData)
	{
		GameObject newObj = Instantiate (initData) as GameObject;
		newObj.transform.parent = parentObj.transform;

		newObj.transform.localScale = new Vector3 (1, 1, 1);
		
		ObjArrayControll (ref targetObj, direction, EditorAct.Add, newObj);
		
		AdjustPositionObj(ref targetObj, direction, EditorAct.Add, newObj);	
		
		RenameGameobjectName (ref targetObj, direction);
	}
	public void DeleteLineObj(ref GameObject[] targetObj, EditorDirect direction, GameObject parentObj)
	{
		int index = (direction == EditorDirect.Up) ? 0 : targetObj.Length - 1;
		
		Destroy (targetObj [index]);
		
		ObjArrayControll (ref targetObj, direction, EditorAct.Remove);
		
		AdjustPositionObj(ref targetObj, direction, EditorAct.Remove);
		
		RenameGameobjectName (ref targetObj, direction);
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
	
	public void AdjustPositionObj(ref GameObject[] editTarget, EditorDirect direction, EditorAct editorAct,
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

	public void ModifyFrame(RectTransform target, bool isPlus)
	{
		int sign = (isPlus == true) ? 1 : -1;
		target.sizeDelta = new Vector2 (target.sizeDelta.x, target.sizeDelta.y + (oneLineDistance * sign));
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
