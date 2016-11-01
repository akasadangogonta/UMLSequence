using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextObj : BaseObj 
{
	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Text;

	[System.NonSerialized]
	public GameObject[] text;
	private Text textCs { get { return text[0].GetComponent<Text> (); } }

	[System.NonSerialized]
	public GameObject[] button;
	
	private TextModifyMethods editMethods;
	
	override protected void AwakeAfter()
	{
		editMethods = this.gameObject.AddComponent<TextModifyMethods> ();
	}

	override protected void InitializeSetParam()
	{
		if (text == null || text.Length == 0)
		{
			text = editMethods.GetTextObj (this.gameObject);
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
		textCs.text = data.text[0];
	}
	
	override protected void AddNewObjBrunch (ObjectsData data = null)
	{
		if (data != null)
		{
			Debug.Log ("Exception argument in AddObj branch class");
			return;
		}

		data.type = (int)thisObjType;

		data.text = new string[1];
		data.text[0] = textCs.text;
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
				tmp[0] = textCs.text;
				item.text = tmp;
				base.UpdateObj(item);
				return;
			}
		}
	}

	override public void ReturnThisScript ()
	{
		GetComponent<ButtonTranspoter> ().script = this;
		GetComponent<ButtonTranspoter> ().type = thisObjType;
	}
}
