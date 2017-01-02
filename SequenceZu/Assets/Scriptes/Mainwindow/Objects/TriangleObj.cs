using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TriangleObj : BaseObj
{
	[System.NonSerialized]
	public ObjType thisObjType = ObjType.Triangle;

	[System.NonSerialized]
	public GameObject[] image;
	private Image imageCs { get { return image[0].GetComponent<Image> (); } }

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
		if (image == null || image.Length == 0)
		{
			image = editMethods.GetImageObj(this.gameObject);
		}
	}

	override protected void Start()
	{
		type = thisObjType;
		base.Start ();
	}
	
	override protected void LoadSaveDataMain(ObjectsData data)
	{
		imageCs.color = editMethods.GetColor(data.Colors[0]); 
	}
	
	override protected void AddNewObjBrunch (ref ObjectsData data)
	{
		if (data == null)
		{
			Debug.Log ("data is null");
			return;
		}

		data.type = (int)thisObjType;
		data.Colors [0] = (int)ColorClass.dic[(Color32)imageCs.color ];//editMethods.ChooseColor(imageCs.color);
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
				int[] tmpColor = new int[1];
				tmpColor[0] = (int)ColorClass.dic [imageCs.color];
				item.Colors = tmpColor;

				base.UpdateObj(item);
				return;
			}
		}
	}
}
