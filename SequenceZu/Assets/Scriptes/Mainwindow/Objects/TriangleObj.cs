using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriangleObj : BaseObj
{
	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Triangle;

	[System.NonSerialized]
	public GameObject[] button;

	private TriangleModifyMethods editMethods;

	override protected void AwakeAfter()
	{
		editMethods = this.gameObject.AddComponent<TriangleModifyMethods> ();
	}
	
	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}

	override protected void InitializeSetParam()
	{
		if (button == null || button.Length == 0)
		{
			button = editMethods.GetButtonObj (this.gameObject);
		}
	}

	override protected void Start()
	{
		type = thisObjType;
		base.Start ();
	}
	
	override protected void LoadSaveDataMain(ObjectsData data)
	{
		//base.LoadSaveData (data);
	}
	
	override protected void AddNewObjBrunch (ObjectsData data = null)
	{
		if (data != null)
		{
			Debug.Log ("Exception argument in AddObj branch class");
			return;
		}

		data.type = (int)thisObjType;
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
				base.UpdateObj(item);
				return;
			}
		}
	}

}
