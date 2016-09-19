using UnityEngine;
using System.Collections;

public class ButtonTranspoter : MonoBehaviour 
{
	[System.NonSerialized]
	public BaseObj script;
	public ObjType type;

	void Awake()
	{
		GetScript ();
	}

	public void GetScript()
	{
		SendMessage ("ReturnThisScript");
		print ("set object type == (int)" + (int)type);
	}

	public void SetDataTranspoter()
	{
		script.SetData ();
	}

	public void LoadSaveDataTrans(ObjectsData data)
	{
		script.LoadSaveData (data);
	}

	public void OnMouseDownTranspoter()
	{
		script.OnMouseDown ();
	}

	public void OnMouseUpTranspoter()
	{
		script.OnMouseUp ();
	}

	public void OnDragTranspoter()
	{
		script.OnDrag ();
	}

	public void OnUpdateObjectsDataToSaveData()
	{
		script.UpdateObjectsDataToSaveData ();
	}
}
