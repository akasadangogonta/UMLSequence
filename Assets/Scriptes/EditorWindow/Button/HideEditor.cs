using UnityEngine;
using System.Collections;

public class HideEditor : MonoBehaviour {
	public void OnClick()
	{
		GeneralController.instance.GetComponent<CreateEditorWindow> ().HideEditorWindow ();
		GeneralController.instance.GetComponent<CreateEditorWindow> ().linkBaseObj.UpdateObjectsDataToSaveData ();
	}
}
