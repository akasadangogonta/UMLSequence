using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AddParts
{
	public List<GameObject> parts = new List<GameObject> ();
}

public class EditorWindowControll : MonoBehaviour {
	private readonly float oneLineDistance = 33.0F;

	public ModifyLineButton[] newLineButton;
	private Vector2[] defaultNewLineButtonPos;
	public ModifyLineButton[] delLineButton;
	private Vector2[] defaultDelLineButtonPos;

	public ColorButton[] colorButton;
	public GameObject[] colorArrowIcon;

	public AngleButton[] angleButton;
	public Text angleText;
	public GameObject colorPointersForAngle;
	public GameObject fluctButtonsForAngle;

	public ScaleButton[] scaleButton;
	public Text scaleText;

	private BaseObj originObj;
	private EditorTargetControllBase targetObj;

	//instantiateParts
	public GameObject[] PartsOfFrameObj;

	private int defaultLineNum = GlobalConfig.defaultFrameLineNum;
	
	void Start()
	{
		defaultNewLineButtonPos = new Vector2[2];
		defaultNewLineButtonPos [0] = newLineButton [0].transform.localPosition;
		defaultNewLineButtonPos [1] = newLineButton [1].transform.localPosition;
		defaultDelLineButtonPos = new Vector2[2];
		defaultDelLineButtonPos [0] = delLineButton [0].transform.localPosition;
		defaultDelLineButtonPos [1] = delLineButton [1].transform.localPosition;
	}

	public void SetData(BaseObj originObj, EditorTargetControllBase targetObj)
	{
		this.targetObj = targetObj;
		this.originObj = originObj;
		targetObj.SetData (originObj, PartsOfFrameObj);
		targetObj.SetCallback (CollorArrowPointerChangePos);

		foreach (var item in colorButton)
		{
			item.SendColorCallback = targetObj.GetColorCallback;
		}

		foreach (var item in newLineButton) 
		{
			item.SetData(targetObj, LineEditButtonChangePos);
		}
		foreach (var item in delLineButton) 
		{
			item.SetData(targetObj, LineEditButtonChangePos);
		}

		foreach (var item in angleButton) 
		{
			item.SetData(targetObj, AngleChange);
		}

		foreach (var item in scaleButton) 
		{
			item.SetData(originObj, ScaleChange);
		}

		ReseLineEditButtonPos ();
		InitializeAngle ();

		if (originObj.type == ObjType.Frame && originObj.lineNum != defaultLineNum)
		{
			int difference = originObj.lineNum - defaultLineNum;

			if (difference > 0)
			{
				for(int count = 0; count < difference; count++)
				{
					LineEditButtonChangePos(true);
				}
			}
			else if (difference < 0)
			{
				difference = Mathf.FloorToInt( Mathf.Abs((float)difference) );
				for(int count = 0; count < difference; count++)
				{
					LineEditButtonChangePos(false);
				}
			}
		}
	}

	private void ReseLineEditButtonPos()
	{
		System.Func<int, Transform> NewBtnTrans = (index) => { return newLineButton [index].transform; };
		System.Func<int, Transform> DelBtnTrans = (index) => { return delLineButton [index].transform; };

		NewBtnTrans (0).localPosition = defaultNewLineButtonPos [0];
		NewBtnTrans (1).localPosition = defaultNewLineButtonPos [1];
		DelBtnTrans (0).localPosition = defaultDelLineButtonPos [0];
		DelBtnTrans (1).localPosition = defaultDelLineButtonPos [1];
	}

	private void LineEditButtonChangePos(bool isLineIncreace)
	{
		System.Func<int, Transform> NewBtnTrans = (index) => { return newLineButton [index].transform; };
		System.Func<int, Transform> DelBtnTrans = (index) => { return delLineButton [index].transform; };

		int sign = (isLineIncreace == true)? 1: -1;

		if (newLineButton.Length == 2)
		{
			NewBtnTrans(0).localPosition = new Vector2(NewBtnTrans(0).localPosition.x,
			                                           NewBtnTrans(0).localPosition.y + (sign * oneLineDistance / 2));
			NewBtnTrans(1).localPosition = new Vector2(NewBtnTrans(1).localPosition.x,
			                                           NewBtnTrans(1).localPosition.y - (sign * oneLineDistance / 2));
		}
		if (delLineButton.Length == 2)
		{
			DelBtnTrans(0).localPosition = new Vector2(DelBtnTrans(0).localPosition.x,
			                                           DelBtnTrans(0).localPosition.y + (sign * oneLineDistance / 2));
			DelBtnTrans(1).localPosition = new Vector2(DelBtnTrans(1).localPosition.x,
			                                           DelBtnTrans(1).localPosition.y - (sign * oneLineDistance / 2));
		}
	}

	private void CollorArrowPointerChangePos(Vector2 leftPos, Vector2 rightPos)
	{
		colorArrowIcon [0].transform.localPosition = leftPos;
		colorArrowIcon [1].transform.localPosition = rightPos;
	}

	private void InitializeAngle()
	{
		System.Action<GameObject> InitializeRotateZ = (target) =>
		{
			var angle = target.transform.rotation.eulerAngles;
			angle.z = 0;
			target.transform.rotation = Quaternion.Euler(angle);
		};

		InitializeRotateZ (colorPointersForAngle);
		InitializeRotateZ (fluctButtonsForAngle);

		var angles = targetObj.transform.rotation.eulerAngles;
		float adjustAngleZ = Mathf.Floor (angles.z * 10) / 10;

		AngleChange (adjustAngleZ);
	}

	private void AngleChange(float value)
	{
		System.Action<GameObject> ActRotateZ = (target) =>
		{
            var angles = target.transform.rotation.eulerAngles;
		
			angles.z = value;
			target.transform.rotation = Quaternion.Euler(angles);
		};

		switch(originObj.type)
		{
		case ObjType.Frame:

			ActRotateZ(colorPointersForAngle);
			ActRotateZ(fluctButtonsForAngle);
			ActRotateZ(originObj.gameObject);
			ActRotateZ(targetObj.gameObject);

			break;
		case ObjType.Triangle:
		case ObjType.Text:
			ActRotateZ(originObj.gameObject);
			ActRotateZ(targetObj.gameObject);

			break;
		}

		if (value >= 360) 
		{
			value -= 360;
		} else if (value < 0) 
		{
			value += 360;
		}
		angleText.text = value.ToString() + "°";
	}

	private void ScaleChange(Vector3 value)
	{
		switch(originObj.type)
		{
		case ObjType.Frame:
		case ObjType.Triangle:
		case ObjType.Text:
			originObj.gameObject.transform.localScale = value;
			//targetObj.gameObject.transform.localScale = value;
			break;
		}

		/*
		if (value >= 360) 
		{
			value -= 360;
		} else if (value < 0) 
		{
			value += 360;
		}
		angleText.text = value.ToString() + "°";
		*/
	}
}
