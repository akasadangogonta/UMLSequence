using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ModifyMethodsBase :MonoBehaviour
{
	public GameObject[] GetTextObj(GameObject target)
	{
		Text[] tmpText = target.GetComponentsInChildren<Text>();
		var tmpTextList = new List<GameObject>();
		foreach (var item in tmpText)
		{
			if (item.gameObject.name.Contains("NameLine"))
			{
				tmpTextList.Add (item.gameObject);
			}
		}
		return tmpTextList.ToArray();
	}
	public GameObject[] GetButtonObj(GameObject target)
	{
		Button[] tmpButton = target.GetComponentsInChildren<Button>();
		var tmpButtonList = new List<GameObject>();
		foreach (var item in tmpButton)
		{
			if (item.gameObject.name.Contains("Button"))
			{
				tmpButtonList.Add (item.gameObject);
			}
		}
		return tmpButtonList.ToArray();
	}
	public GameObject[] GetInputObj(GameObject target)
	{
		InputField[] tmpInput = target.GetComponentsInChildren<InputField>();
		var tmpInputList = new List<GameObject>();
		foreach (var item in tmpInput)
		{
			if (item.gameObject.name.Contains("InputField"))
			{
				tmpInputList.Add (item.gameObject);
			}
		}
		return tmpInputList.ToArray();
	}
}
