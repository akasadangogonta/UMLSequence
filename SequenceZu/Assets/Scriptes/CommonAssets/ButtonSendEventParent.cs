using UnityEngine;
using System.Collections;


public class ButtonSendEventParent : AutoAddEventTrigger
{
	public enum ButtonEvent
	{
		OnMouseDown,
		OnMouseUp,
		OnEnter,
	}

	public ButtonEvent[] buttonEvent;

	override public void OnMouseDown()
	{
		foreach (var item in buttonEvent)
		{
			if(item == ButtonEvent.OnMouseDown)
			{
				this.transform.parent.SendMessage("OnMouseDown");
				return;
			}
		}
	}

	override public void OnEnter()
	{
		foreach (var item in buttonEvent)
		{
			if(item == ButtonEvent.OnEnter)
			{
				this.transform.parent.SendMessage("OnEnter");
				return;
			}
		}
	}
}
