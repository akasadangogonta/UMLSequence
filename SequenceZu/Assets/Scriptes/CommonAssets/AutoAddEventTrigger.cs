using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class AutoAddEventTrigger : MonoBehaviour {
	protected EventTrigger 		trigger;
	protected EventTrigger.Entry  entry;

	void Awake()
	{
		AwakeBefore ();
		AwakeMain ();
	}
	private void AwakeBefore()
	{
		if (GetComponent<EventTrigger> () == null) 
		{
			this.gameObject.AddComponent<EventTrigger> ();
			trigger = GetComponent<EventTrigger> ();
			trigger.delegates = new List<EventTrigger.Entry> ();
		}
		else
		{
			trigger = GetComponent<EventTrigger> ();
		}
	}
	virtual protected void AwakeMain () {}

	
	
	virtual protected void Start () 
	{
		entry = new EventTrigger.Entry ();
		
		AddListener (EventTriggerType.PointerDown, OnMouseDown);
		AddListener (EventTriggerType.PointerUp, OnMouseUp);
		AddListener (EventTriggerType.Drag, OnDrag);
		AddListener (EventTriggerType.PointerEnter, OnEnter);
		AddListener (EventTriggerType.PointerExit, OnExit);
		AddListener (EventTriggerType.PointerClick, OnClick);
	}

	private void AddListener(EventTriggerType type, Action action)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = type;
		
		entry.callback.AddListener ((callback) =>
	    {
			action();
		});
		trigger.delegates.Add (entry);
	}

	//
	//SetEvent
	//
	virtual public void OnMouseDown()
	{

	}

	virtual public void OnMouseUp()
	{

	}

	virtual public void OnDrag()
	{

	}

	virtual public void OnEnter()
	{
		
	}
	
	virtual public void OnExit()
	{
		
	}

	virtual public void OnClick()
	{
		
	}

}
