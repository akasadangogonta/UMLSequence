using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum WheelDirect
{
	Back,
	ThisSide
}

public class MouseWheel : MonoBehaviour 
{
	public Canvas[]    targetCanvas;
	public Camera	mainCamera;
	public Text[] 		scaleViewText;

	private readonly float rollActAchieveValue = 0;//0.2F; //ロール操作を受け入れる達成値
	private readonly float rollActAcceptLimit = 0.1F;//0.1F; //ロールした値を+=できる期間
	private readonly double reAcceptTimeValue = 0F; //再ロールを受け入れるまでのスパン
	private double reAcceptTime = 0F;
	private WheelDirect    wheelDirect = WheelDirect.Back;
	private double touchMoment;

	private MouseEvent mouseEvent = MouseEvent.None;
	private float      mouseAxis = 0;

	private bool isChange = false;

	private float GetScale { get { return GeneralController.GetScale; } }

	// Use this for initialization
	void Start () {
		touchMoment = Time.time;

		mainCamera = GameObject.Find ("/Main Camera").GetComponent<Camera>();
		/*
		foreach (var item in targetCanvas)
		{
			item.transform.localScale = new Vector3(GetScale ,
			                                        GetScale ,
			                                        item.transform.localScale.z);
		}
*/
		ChangeView ();
	}
	
	void Update () 
	{
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) 
		{
			WheelEvent();
		} 
	}


	private void ChangeView()
	{
		foreach (var item in scaleViewText)
		{
			item.text = GetScale.ToString();
		}
		isChange = false;
	}

	private void WheelEvent()
	{
		reAcceptTime -= (Time.time - touchMoment);
		if (reAcceptTime > 0)
		{
			return;
		}

		WheelDirect preDirect = wheelDirect;
		float	    preMouseAxis = mouseAxis;
		mouseAxis = Input.GetAxis ("Mouse ScrollWheel");

		if (mouseAxis > rollActAchieveValue) 
		{
			wheelDirect = WheelDirect.Back;
		}
		else if (mouseAxis < -rollActAchieveValue)
		{
			wheelDirect = WheelDirect.ThisSide;
		}

		mouseAxis = preMouseAxis;

		if (preDirect == wheelDirect &&
			Time.time - touchMoment < rollActAcceptLimit)
		{
			mouseAxis += Input.GetAxis ("Mouse ScrollWheel") ;
		}

		Action<WheelDirect> ScaleChange = (direct) =>
		{
			float baseChangeValue = 1F;
			if (direct == WheelDirect.Back)
			{
				baseChangeValue = baseChangeValue * 1;
			}
			else if (direct == WheelDirect.ThisSide)
			{
				baseChangeValue = baseChangeValue * -1;
			}

			//GeneralController.SetScale(GetScale + baseChangeValue);
			float scaleValue = (Mathf.Round(GetScale + baseChangeValue));

			if (scaleValue < 1F)
			{
				scaleValue = 1F;
			}
			if (scaleValue > 20F)
			{
				scaleValue = 20F;
			}
			GeneralController.SetScale(scaleValue);

			mainCamera.orthographicSize = scaleValue;
			/*
			foreach (var item in targetCanvas)
			{
				item.transform.localScale = new Vector3(GetScale,
				                                        GetScale,
				                                        item.transform.localScale.z);
			}*/

			mouseAxis = 0;
			reAcceptTime = reAcceptTimeValue;
			ChangeView();
		};

		if (mouseAxis > rollActAchieveValue) 
		{
			print ("Back");	
			ScaleChange(WheelDirect.Back);
		}
		else if (mouseAxis < -rollActAchieveValue)
		{
			print ("ThisSide");
			ScaleChange(WheelDirect.ThisSide);
		}
		
		touchMoment = Time.time;
	}
}
