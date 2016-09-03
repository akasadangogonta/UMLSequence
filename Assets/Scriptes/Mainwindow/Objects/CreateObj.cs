using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;


public enum ObjType
{
	None = -1,
	Frame = 0,
	Triangle,
	TriangleWire,
	Text,
}

public class CreateObj : MonoBehaviour  {
	public GameObject[] createObjectsType;
	public GameObject[] createPartsType;
	private GameObject[] instanceCreateObjects;
	
	public GameObject targetObjType;
	private GameObject createArea;
	private GameObject instanceTargetObj;


	protected long GetCurId { get { return GeneralController.GetCurId; } }

	// Use this for initialization
	void Awake () {
		Func <CreateObj> General = () =>
		{
			return GameObject.Find ("/GeneralController").GetComponent<CreateObj>();
		};

		if (General () != null)
		{
			this.createObjectsType = General().createObjectsType;
		}
	}

	void Start () {
		createArea = GameObject.Find ("/MainCanvas/CreateArea");
	}

	
	public void ClickButton()
	{
		GeneralController.SetCurId (GetCurId + 1);
		
		InstantiateObj ();
		
		instanceTargetObj.GetComponent<ButtonTranspoter>().SetDataTranspoter();
	}

	public void _GetInstanceGameObjectFromObjectData(ObjectsData data)
	{
		foreach (var item in GeneralController.instance.GetComponent<CreateObj>().instanceCreateObjects)
		{
			ObjType tmpType =  item.GetComponent<ButtonTranspoter>().type;
			Debug.Log ("searching object type == " + tmpType);
			if(tmpType == (ObjType)data.type)
			{
				Debug.Log ("hit object type == " + tmpType);
				targetObjType = item;
				break;
			}
		}

		if (targetObjType == null)
		{
			Debug.LogWarning("not found this argument type");
			return ;
		}

		InstantiateObj ();
	}

	public void InstantiateObj()
	{
		instanceTargetObj = Instantiate (targetObjType) as GameObject;
		instanceTargetObj.transform.parent = GameObject.Find ("/MainCanvas/CreateArea").transform;
		instanceTargetObj.transform.localPosition = new Vector3 (0, 0, 0);
		instanceTargetObj.transform.localScale = new Vector2 (1, 1);
	}

	public GameObject Create(ObjectsData data)
	{
		_GetInstanceGameObjectFromObjectData (data);
		instanceTargetObj.GetComponent<ButtonTranspoter>().LoadSaveDataTrans(data);

		return instanceTargetObj;
	}


	public List<BaseObj> LoadDataToCreateObj()
	{
		if (GeneralController.m_savedata.m_obj == null ||
			GeneralController.m_savedata.m_obj.Length <= 0)
		{
			return null;
		}

		if (instanceCreateObjects == null) 
		{
			int length = createObjectsType.Length;
			instanceCreateObjects = new GameObject[createObjectsType.Length];
			for (int count = 0; count < createObjectsType.Length; count++)
			{
				instanceCreateObjects[count] = Instantiate (createObjectsType[count]) as GameObject;
			}
		}
		
		CreateObj createObj = GetComponent<CreateObj> ();
		List<BaseObj> instantiatedObj = new List<BaseObj>();

		foreach (var item in GeneralController.m_savedata.m_obj)
		{


			createObj.Create (item);

			instantiatedObj.Add(instanceTargetObj.GetComponent<ButtonTranspoter>().script);
		}

		if (instantiatedObj.Count != 0) 
		{
			return instantiatedObj;
		} 
		else 
		{
			return null;
		}
	}
}
