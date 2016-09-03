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


	EditFrameMethods editMethods = new EditFrameMethods ();

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
			//base.lineNum = text.Length;
		}
	}
	
	override protected void Start()
	{
		base.Start ();

		type = thisObjType;
		int defaultFrameLineNum = 2;
		lineNum = defaultFrameLineNum;
	}

	override public void LoadSaveData(ObjectsData data)
	{
		base.LoadSaveData (data);
		print ("いがあああ lineNum = " + data.lineNum);
		if (data.lineNum == 3)
		{
			print ("TRUEEEEEeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

		}

		for (int count = 0; count < text.Length; count++)
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
				print ("いがあああ text.Length" + text.Length);
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







	/*


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
*/
}
