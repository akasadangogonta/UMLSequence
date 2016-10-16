using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextObj : BaseObj 
{
	public Text line;

	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Text;

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
	
	override protected void LoadSaveDataMain(ObjectsData data)
	{
		//line.text = data.text[0];

		//base.LoadSaveData (data);
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

		data.text = new string[1];
		data.text[0] = line.text;
		
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
				string[] tmp = new string[1];
				tmp[0] = line.text;
				item.text = tmp;
				base.UpdateObj(item);
				return;
			}
		}
	}
}
