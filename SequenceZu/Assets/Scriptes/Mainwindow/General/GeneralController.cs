using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GeneralData
{
	public long curId;
	public float scale;
}

public class GeneralController : MonoBehaviour {
	static public GeneralController instance;
	static public GameObject instanceThisGameObject;

	static public savedata m_savedata;
	static private long curId;
	static private float scale;

	private bool isChange = false;

	static public List<BaseObj> instatiatedObj;

	public GameObject curtain;

	void Awake()
	{
		curId = 0;
		scale = 5F;

		ColorClass.Initialize ();

		instatiatedObj = new List<BaseObj> ();

		m_savedata = record.load ();
		if (m_savedata != null )
		{
			if (m_savedata.m_general != null)
			{
				if (m_savedata.m_general.curId <= 0)
				{
					m_savedata.m_general.curId = 0;
				}
				if (m_savedata.m_general.scale <= 0)
				{
					m_savedata.m_general.scale = scale;
				}
			}
			else
			{
				curId = m_savedata.m_general.curId;
				scale = m_savedata.m_general.scale;
			}
		}

		instance = this;
		instanceThisGameObject = this.gameObject;
	}
	
	void Start()
	{
		if (m_savedata.m_obj != null)
		{
			List<BaseObj> obj =  GetComponent<CreateObjControll>().LoadDataToCreateObj () ;
			if (obj != null)
			{
				instatiatedObj = obj;
			}
		}

		foreach (var item in instatiatedObj)
		{
			//item.SetId(0);
		}

		isChange = true;
	}

	void Update()
	{
		if (isChange)
		{
			isChange = false;
		}
	}

    public void SetCurtain(bool flag)
	{
		curtain.SetActive (flag);
		GetComponent<MouseWheel>().enabled = !flag;
	}

	static public long GetCurId
	{
		get { return m_savedata.m_general.curId; }
	}
	static public void SetCurId(long value)
	{
		m_savedata.m_general.curId = value;
	}

	static public float GetScale
	{
		get { return m_savedata.m_general.scale; }
	}
	static public void SetScale(float value)
	{
		m_savedata.m_general.scale = value;
	}

	static public void SetNewObject(ObjectsData data, BaseObj obj)
	{
		int length = m_savedata.m_obj.Length;
		ObjectsData[] tmpArray = new ObjectsData[length + 1];
		
		for (int count = 0; count < length + 1; count++) 
		{
			if (count == length)
			{
				tmpArray [count] = data;
			} 
			else 
			{
				tmpArray [count] = m_savedata.m_obj [count];
			}
		}
		
		m_savedata.m_obj = tmpArray;
		instatiatedObj.Add (obj);
	}

	static public void RemoveObject(long id)
	{
		var tmpObjList = new List<ObjectsData> ();
		tmpObjList.AddRange (m_savedata.m_obj);

		ObjectsData tmpA = null;
		foreach (var item in tmpObjList)
		{
			if(item.id == id)
			{
				tmpA = item;
			}
			if(item.id > id)
			{
				item.id--;
			}
		}
		if (tmpA != null) 
		{
			tmpObjList.Remove(tmpA);
			m_savedata.m_general.curId--;
		}
		m_savedata.m_obj = tmpObjList.ToArray ();


		BaseObj tmpB = null;
		foreach (var item in instatiatedObj)
		{
			if(item.ID == id)
			{
				tmpB = item;
			}
			if(item.ID > id)
			{
				item.ID--;
			}
		}
		if (tmpB != null) 
		{
			instatiatedObj.Remove(tmpB);
		}

		curId--;
	}

	private ObjectsData _GetSelectedObjectData(long id)
	{
		foreach (var item in m_savedata.m_obj)
		{
			if(item.id == id)
			{
				return item;
			}
		}

		Debug.LogWarning ("not found this arugument id Object");
		return null;
	}

	public GameObject GetSelectTargetObject(long id)
	{
		ObjectsData data = _GetSelectedObjectData (id);
		return GetComponent<CreateObjControll> ().Create (data);
	}
}
