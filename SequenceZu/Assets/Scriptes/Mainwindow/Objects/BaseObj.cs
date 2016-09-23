using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum MouseEvent
{
	None = -1,
	Left = 0,
	Right,
	Center
}

[System.Serializable]
public class ObjectsData
{
	public struct _color
	{
		int r;
		int g;
		int b;
		int a;
	}

	public long id;
	public int type;
	public int lineNum;
	public float angle;
	public float scaling;
	public float posX;
	public float posY;
	public string[] text;
	public int[] color;
}

public class BaseObj : AutoAddEventTrigger
{
	protected long id;
	public long ID { get { return id; } set { id = value; } }
	public ObjType type { get; protected set; }

	[System.NonSerialized]
	public int lineNum;

	//private Vector3 beforePos;
	//private Vector3 afterPos;
	private readonly float gridSizeInnerValue = 4.5F;

	private float gridSizeInner = 0;
	private float gridSizeBase = 0;
	//private float GeneralContorller.scale = 0.009F;
	private Color[] defaultColor;
	
	private double touchMoment;
	private float doubleClickLimit = 0.3F;
	
	private bool alreadyLoop = false;
	private bool alreadyUpdate = false;
	private bool dontDestroy = false;

	private GameObject movingImage;

	private static MouseEvent mouseEvent = MouseEvent.None;
	public static void SetMouseEvent(MouseEvent mouseEvent)
	{
		BaseObj.mouseEvent = mouseEvent;
	}


	protected long GetCurId { get { return GeneralController.GetCurId; } }
	protected float GetScale { get { return GeneralController.GetScale; } }
	
	virtual public void ReturnThisScript()
	{
	}

	virtual protected void InitializeSetParam()
	{
	}

	virtual public void LoadSaveData(ObjectsData data)
	{
		InitializeSetParam ();

		this.id = data.id;
		this.lineNum = data.lineNum;
		Vector3 position = new Vector3 (data.posX, data.posY, 1);
		this.gameObject.transform.localPosition = position;

		this.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, data.angle));
		//UpdateObj ();
	}

	virtual protected void AddNewObj(ObjectsData data = null)
	{
		if (data == null)
		{
			Debug.Log ("Exception argument in AddObj base class");
			return;
		}
		InitializeSetParam ();
		
		data.id = GetCurId;
		data.lineNum = 0;
		data.posX= this.transform.localPosition.x;
		data.posY= this.transform.localPosition.y;

		data.angle = this.transform.rotation.eulerAngles.z;
		
		GeneralController.SetNewObject (data, this);
	}

	virtual protected void UpdateObj (ObjectsData data = null)
	{
		if (data == null)
		{
			Debug.Log ("Exception argument in UpdateObj base class");
			return;
		}
		
		data.posX= this.transform.localPosition.x;
		Debug.Log ("save data.posx = " + data.posX);
		data.posY= this.transform.localPosition.y;
		Debug.Log ("save data.posy = " + data.posY);
		
		data.angle = this.transform.rotation.eulerAngles.z;
		Debug.Log ("save data.angle = " + data.angle);
		return;
	}
	
	public void UpdateObjectsDataToSaveData()
	{
		UpdateObj ();
	}

	override protected void Awake () 
	{
		base.Awake ();
		touchMoment = Time.time;
	}

	override protected void Start()
	{
		base.Start ();

		InitializeSetParam();
		gridSizeBase = (gridSizeInnerValue / 0.008F / 100);
		ChangeGridInnner ();
	}

	public void SetData()
	{
		this.id = GetCurId;
		AddNewObj ();
	}
		
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			PushEvent(MouseEvent.Left);
		} 
		else if (Input.GetMouseButtonDown (1))
		{
			PushEvent(MouseEvent.Right);
		} 
		else if (Input.GetMouseButtonDown (2)) 
		{
			PushEvent(MouseEvent.Center);
		}
		alreadyUpdate = true;
	}
	private void PushEvent(MouseEvent mouseEvent)
	{
		if (OutArea ()) 
		{
			BaseObj.mouseEvent = MouseEvent.None;
			return;	
		}
		BaseObj.mouseEvent = mouseEvent;
	}

	private bool OutArea()
	{
		Vector3 curMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 targetPos = this.transform.position;
		Vector2 targetSize = this.GetComponent<RectTransform> ().sizeDelta;
		
		if (curMousePos.x > targetPos.x - ((targetSize.x * GetScale) / 2) &&
		    curMousePos.x < targetPos.x + ((targetSize.x * GetScale) / 2) &&
		    curMousePos.y > targetPos.y - ((targetSize.y * GetScale) / 2) &&
		    curMousePos.y < targetPos.y + ((targetSize.y * GetScale) / 2))
		{
			return false;
		}
		return true;
	}

	private void ChangeGridInnner()
	{
		gridSizeInner = (gridSizeInnerValue / GetScale / 100);
	}

	override public void OnMouseDown()
	{ 
		Debug.Log ("touched ID = " + id);

		if (BaseObj.mouseEvent == MouseEvent.None ||
			BaseObj.mouseEvent == MouseEvent.Center) 
		{
			if (alreadyLoop == false) {
				Invoke ("OnMouseDown", 0.01F);
				alreadyLoop = true;
				return;
			}
		} 
		else if (alreadyUpdate = false)
		{
			Invoke ("OnMouseDown", 0.01F);
			return;
		}
		else if (BaseObj.mouseEvent == MouseEvent.Left)
		{
			dontDestroy = false;

			ActionDoubleClick(OnDoubleClickLeft);
		}
		else if (BaseObj.mouseEvent == MouseEvent.Right)
		{
			dontDestroy = true;

			ActionDoubleClick(OnDoubleClickRight);
		}
		else if (BaseObj.mouseEvent == MouseEvent.Right)
		{
			BaseObj.mouseEvent = MouseEvent.None;
		}

		alreadyLoop = false;
		alreadyUpdate = false;
	}

	override public void OnMouseUp()
	{
		if (BaseObj.mouseEvent == MouseEvent.None)
		{
			return;
		}

		Vector3 vecMouse = Input.mousePosition;
		Vector3 screenPos = Camera.main.ScreenToWorldPoint(vecMouse);

		if (movingImage != null && dontDestroy == false)
		{
			this.transform.localPosition = movingImage.transform.localPosition;
			Destroy (movingImage);
			UpdateObj();
		}
		else if (movingImage != null && dontDestroy == true)
		{
			SetTransperent(movingImage);
			AddCopyObjToSaveData();

			movingImage = null;
			dontDestroy = false;
		}

		BaseObj.mouseEvent = MouseEvent.None;
	}

	override public void OnDrag()
	{
		if (BaseObj.mouseEvent == MouseEvent.Left ||
			BaseObj.mouseEvent == MouseEvent.Right)
		{
			DrawMovingImage (this.gameObject);
		}
	}


	private void ActionDoubleClick(Action act)
	{
		if ((Time.time - touchMoment) < doubleClickLimit)
		{
			act();
		}
		touchMoment = Time.time;
	}

	private void OnDoubleClickLeft()
	{
		print ("DoubleClickLeft");
		
		GeneralController.instanceThisGameObject.GetComponent<CreateEditorWindow> ().ShowEditorWindow (this);
	}

	private void OnDoubleClickRight()
	{
		print ("DoubleClickRight");

		GeneralController.RemoveObject (this.id);
		Destroy (this.gameObject);
	}

	//inner process
	protected void DrawMovingImage(GameObject targetObj)
	{
		if (movingImage == null) 
		{
			movingImage = Instantiate (targetObj) as GameObject;
			movingImage.transform.parent = targetObj.transform.parent.transform;
			movingImage.transform.localScale = new Vector2 (1, 1);
			movingImage.transform.position = targetObj.transform.position;

			SetTransperent(movingImage, 50);
		}

		Vector3 vecMouse = Input.mousePosition;
		Vector3 screenPos = Camera.main.ScreenToWorldPoint(vecMouse);
	
		ChangeGridInnner ();

		float ceilX = Mathf.Round (screenPos.x * gridSizeBase);
		float ceilY = Mathf.Round (screenPos.y * gridSizeBase);

		Vector2 ceilPos = new Vector2 (ceilX / gridSizeBase,
		                               ceilY / gridSizeBase);

		movingImage.transform.position = ceilPos;
	}

	private void SetTransperent(GameObject targetObj, float percent)
	{
		SaveObjColor(targetObj);
		_SetTransperent (targetObj, percent);
	}
	private void SetTransperent(GameObject targetObj)
	{
		_SetTransperent (targetObj, 100);
	}
	public void _SetTransperent(GameObject targetObj, float percent)
	{
		Text[] targetText = targetObj.GetComponentsInChildren<Text>();
		Image[] targetImage = targetObj.GetComponentsInChildren<Image>();
		
		int i = 0;
		if (targetText != null) 
		{
			foreach (var item in targetText)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b,
				                       defaultColor[i].a * (percent / 100));
				i++;
			}
		}
		if (targetImage != null) 
		{
			foreach (var item in targetImage)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b,
				                       defaultColor[i].a * (percent / 100));
				i++;
			}
		}
	}

	public void SaveObjColor(GameObject targetObj)
	{
		Text[] targetText = targetObj.GetComponentsInChildren<Text>();
		Image[] targetImage = targetObj.GetComponentsInChildren<Image>();

		int arrayNum = 0;
		arrayNum += (targetText != null) ? targetText.Length : 0;
		arrayNum += (targetImage != null) ? targetImage.Length : 0;

		defaultColor = new Color[arrayNum];
		int i = 0;
		
		if (targetText != null) 
		{
			foreach (var item in targetText)
			{
				defaultColor[i] = item.color;
				i++;
			}
		}
		if (targetImage != null) 
		{
			foreach (var item in targetImage)
			{
				defaultColor[i] = item.color;
				i++;
			}
		}
	}

	private void AddCopyObjToSaveData()
	{
		GeneralController.SetCurId (GetCurId + 1);
		
		movingImage.GetComponent<ButtonTranspoter>().SetDataTranspoter();

		foreach (var item in GeneralController.instatiatedObj) 
		{

		}
	}

}
