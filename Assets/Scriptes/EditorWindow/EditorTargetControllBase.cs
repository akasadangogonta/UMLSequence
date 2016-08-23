using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public struct CurEditData
{
	public int id;
	public EditType editType;
}

public class EditorTargetControllBase : MonoBehaviour {


	//protected ObjType type;
	protected BaseObj baseObj;

	public ObjType type;

	protected CurEditData curEditData;

	//instantiate parts
	protected GameObject partsButton;
	protected GameObject partsNewLine;

	protected AddParts addParts;

	public void SetData(BaseObj obj, AddParts addParts)
	{
		this.baseObj = obj;
		this.addParts = addParts;
	}

	virtual protected void Start ()
	{
		if (GetComponent<ButtonTranspoter> () == null) 
		{
			Debug.LogWarning ("not attouch <ButtonTranspoter> script");
		}
		else if (GetComponent<ButtonTranspoter> ().type == null)
		{
			Debug.LogWarning ("not set type data");
		}
		else 
		{
			//type = GetComponent<ButtonTranspoter> ().type;
		}

		curEditData.editType = EditType.None;
		curEditData.id = 0;
		type = baseObj.type;
	}

	protected void SetAddListener(GameObject[] target, EditType type)
	{
		for(int i = 0; i < target.Length; i++)
		{
			if(target[i].GetComponent<TouchObj>() == null)
			{
				target[i].AddComponent<TouchObj>();
			}
			target[i].GetComponent<TouchObj>().SetData(i, type, TouchCallBack);
		}
	}

	protected void TouchCallBack(int touchId, EditType editType)
	{
		curEditData.id = touchId;
		curEditData.editType = editType;
	}

	virtual public void GetColorCallback(Color32[] color32)
	{
	}
}
