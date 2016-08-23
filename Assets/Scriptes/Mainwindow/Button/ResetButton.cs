using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	public void ClickButton()
	{
		PlayerPrefs.DeleteAll ();
	}
}
