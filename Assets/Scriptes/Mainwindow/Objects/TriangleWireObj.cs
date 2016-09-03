using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriangleWireObj : BaseObj
{
	[System.NonSerialized]
	public ObjType thisObjType = ObjType.TriangleWire;

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}

	override protected void Start()
	{
		type = thisObjType;
		base.Start ();
	}
	
	override public void LoadSaveData(ObjectsData data)
	{
		base.LoadSaveData (data);
	}
	
	override protected void AddNewObj (ObjectsData data = null)
	{
		if (data != null)
		{
			Debug.Log ("Exception argument in AddObj branch class");
			return;
		}
		
		data = new ObjectsData ();
		data.type = (int)thisObjType;

		base.AddNewObj(data);
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
