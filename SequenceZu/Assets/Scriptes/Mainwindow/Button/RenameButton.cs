using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class RenameButton : AutoAddEventTrigger {
	public Text renameTarget;
	public GameObject edit;
	private int editSibling = 0;

	public int id;
	private string inputedText;
	bool isChange = false;
	
	private void Start ()
	{
		if (edit != null) {
			ChgActiveInputField(false);
			SetCallback();
		}

		if (renameTarget != null)
		inputedText = renameTarget.text;

		base.Start ();
	}

	private void SetCallback()
	{
		edit.GetComponent<InputField> ().onEndEdit.AddListener ((callback) =>
		{
			OnEndEdit ();
		});
	}

	void Update ()
	{
		if (isChange == true) 
		{
			CloseInputView();
			isChange = false;
		}
	}

	private void OnEndEdit()
	{
		renameTarget.text = edit.GetComponent<InputField> ().text;
		
		if (renameTarget.text == "" || renameTarget.text == null) 
		{
			renameTarget.text = inputedText;
		} 
		else
		{
			inputedText = renameTarget.text;
		}
		edit.GetComponent<InputField> ().DeactivateInputField ();

		int lineLimit = 25;
		if((int)inputedText.Length / lineLimit >= 1 &&
		    this.transform.parent.GetComponent<ButtonTranspoter> ().type == ObjType.Frame)
		{
			if(id > 0)
			{
				FrameObj frame = this.transform.parent.GetComponent<FrameObj>();
				if (frame != null)
				{
					frame.ExpandForInputLine(id);
				}
			}
		}

		this.transform.parent.GetComponent<ButtonTranspoter> ().OnUpdateObjectsDataToSaveData ();
		isChange = true;
	}

	private void CloseInputView()
	{
		ChgActiveInputField(false);
		edit.transform.SetSiblingIndex (editSibling);
	}

	override public void OnMouseDown()
	{
		this.transform.parent.GetComponent<ButtonTranspoter> ().OnMouseDownTranspoter ();
	}
	
	override public void OnMouseUp()
	{
		this.transform.parent.GetComponent<ButtonTranspoter> ().OnMouseUpTranspoter ();
	}
	
	override public void OnDrag()
	{
		this.transform.parent.GetComponent<ButtonTranspoter> ().OnDragTranspoter ();
	}

	override public void OnClick()
	{
		ChgActiveInputField(true);
		edit.GetComponent<InputField> ().ActivateInputField ();

		editSibling = edit.transform.GetSiblingIndex ();
		edit.transform.SetAsLastSibling ();
	}

	private void ChgActiveInputField(bool flag)
	{
		edit.GetComponent<InputField> ().interactable = flag;
		System.Func<Text[]> GetText = () => { return edit.GetComponentsInChildren<Text> (); };

		foreach (var item in GetText())
		{
			if(item.gameObject.name == "Text")
			{
				item.enabled = flag;
			}
		}
	}
}
