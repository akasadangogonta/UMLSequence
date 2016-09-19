using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EditType
{
	None = 0,
	Text,
	Button,
	Image,
}


public class TouchObj : AutoAddEventTrigger {
	private int touchId;
	private EditType editType;
	Action<int, EditType> touchCallback;

	public void SetData(int id, EditType editType, Action<int, EditType> act)
	{
		this.touchId = id;
		this.editType = editType;
		touchCallback = act;
	}
	
	//
	//SetEvent
	//
	override public void OnMouseDown()
	{
		touchCallback (touchId , editType);
	}

}
