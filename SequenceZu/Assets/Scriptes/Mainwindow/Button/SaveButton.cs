using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour {

	public void ClickButton()
	{
		record.save (GeneralController.m_savedata);
	}
}
