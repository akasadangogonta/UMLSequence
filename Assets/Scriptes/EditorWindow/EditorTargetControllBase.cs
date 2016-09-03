using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public struct CurEditData
{
	public int id;
	public EditType editType;
}

public class EditorTargetControllBase : MonoBehaviour {
	protected int lineNum;
	public int LineNum { get { return lineNum; } }

	protected BaseObj baseObj;

	public ObjType type;

	protected CurEditData curEditData;

	//instantiate parts
	protected GameObject partsButton;
	protected GameObject partsNewLine;

	protected GameObject[] addPartsOfFrameObj;

	public delegate void ColorArrowPosCallback(Vector2 leftPos, Vector2 rightPos);
	protected ColorArrowPosCallback collorArrowPosCallback;
	
	public void SetData(BaseObj obj, GameObject[] addPartsOfFrameObj)
	{
		this.baseObj = obj;
		this.addPartsOfFrameObj = addPartsOfFrameObj;
	}

	public void SetCallback(ColorArrowPosCallback callback)
	{
		this.collorArrowPosCallback = callback;
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

		curEditData.editType = EditType.Text;
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

		ChangeColorAllowPos ();
	}

	virtual public void GetColorCallback(Color32[] color32)
	{
	}

	virtual protected void  ChangeColorAllowPos()
	{

	}
}
