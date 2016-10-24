using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CreateEditorWindow : MonoBehaviour
{
	public GameObject instanceEditorWindow;
	public GameObject InnerCircle;

	[System.NonSerialized]
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
		instanceTargetObj.transform.SetSiblingIndex (7);
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

			instanceTargetObj.AddComponent<EditorTargetControllOnFrame>();

			instanceEditorWindow.GetComponent<EditorWindowControll>().SetData
				(data, instanceTargetObj.GetComponent<EditorTargetControllOnFrame>());
			break;
		case ObjType.Triangle:
			Destroy(instanceTargetObj.GetComponent<TriangleObj>());

			instanceTargetObj.AddComponent<EditorTargetControllOnTriangle>();
			
			instanceEditorWindow.GetComponent<EditorWindowControll>().SetData
				(data, instanceTargetObj.GetComponent<EditorTargetControllOnTriangle>());
			break;
		case ObjType.Text:
			Destroy(instanceTargetObj.GetComponent<TextObj>());
			
			instanceTargetObj.AddComponent<EditorTargetControllOnText>();
			
			instanceEditorWindow.GetComponent<EditorWindowControll>().SetData
				(data, instanceTargetObj.GetComponent<EditorTargetControllOnText>());
			break;
		}

		GeneralController.instance.SetCurtain (true);
	}

	public void HideEditorWindow()
	{
		instanceEditorWindow.SetActive(false);
		Destroy (instanceTargetObj);

		BaseObj.SetMouseEvent (MouseEvent.None);

		GeneralController.instance.SetCurtain (false);
	}
}
