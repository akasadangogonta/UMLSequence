using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CreateEditorWindow : MonoBehaviour
{
	public GameObject instanceEditorWindow;
	public GameObject InnerCircle;
	public GameObject instanceTargetObj;

	[System.NonSerialized]
	public BaseObj linkBaseObj;

	void Start()
	{
		instanceEditorWindow.SetActive(false);
	}

	public void ShowEditorWindow(BaseObj data)
	{
		linkBaseObj = data;

		instanceEditorWindow.SetActive(true);

		instanceTargetObj = Instantiate (data.gameObject) as GameObject;
		instanceTargetObj.transform.parent = instanceEditorWindow.transform;
		Vector3 screenPos = Camera.main.ScreenToWorldPoint(InnerCircle.transform.localPosition );
		instanceTargetObj.transform.position = InnerCircle.transform.position;
		instanceTargetObj.transform.localScale = new Vector3 (1, 1, 1);

		switch (data.type)
		{
		case ObjType.Frame:
			Destroy(instanceTargetObj.GetComponent<FrameObj>());

			var itemButton = instanceTargetObj.GetComponentsInChildren<Button>();
			foreach (var item in itemButton) 
			{
				item.interactable = false;
			}


			var itemRenameButton = instanceTargetObj.GetComponentsInChildren<RenameButton>();
			foreach (var item in itemRenameButton) 
			{
				Destroy(item);
			}
			var itemButtonSupport = instanceTargetObj.GetComponentsInChildren<ButtonSupport>();
			foreach (var item in itemButtonSupport) 
			{
				Destroy(item);
			}
			/*
			var itemEventTrigger = instanceTargetObj.GetComponentsInChildren<EventTrigger>();
			foreach (var item in itemEventTrigger) 
			{
				//Destroy(item);
			}
			*/

			instanceTargetObj.AddComponent<EditorTagetControllOnFrame>();

			instanceEditorWindow.GetComponent<EditorWindowControll>().SetData
				(data, instanceTargetObj.GetComponent<EditorTagetControllOnFrame>());
			break;
		}
	}

	public void HideEditorWindow()
	{
		instanceEditorWindow.SetActive(false);
		Destroy (instanceTargetObj);

		BaseObj.SetMouseEvent (MouseEvent.None);
	}
}
