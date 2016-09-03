using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour {

	public void ClickButton()
	{
		record.save (GeneralController.m_savedata);

		/*
		foreach (var item in GeneralController.m_savedata.m_obj)
		{
			print ("いがああああ" + item.posX);
		}*/
	}
}
