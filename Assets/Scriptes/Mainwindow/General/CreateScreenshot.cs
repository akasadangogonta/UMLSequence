using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CreateScreenshot : MonoBehaviour 
{
	public Canvas[] hideCanvas;
	private string path;
	private bool isChange = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isChange == true)
		{
			if(File.Exists(path) == true)
			{
				foreach (var item in hideCanvas) 
				{
					item.enabled = true;
				}
				isChange = false;
			}
		}
	}

	public void ButtonClick()
	{
		if (isChange == false) 
		{
			foreach (var item in hideCanvas) 
			{
				item.enabled = false;
			}

			path = "image" + Time.time + ".png";

			Application.CaptureScreenshot (path); 

			#if UNITY_EDITOR
			path = path;
			#elif UNITY_STANDALONE
			string tmp = Application.dataPath;
			path = tmp + "/" + path;
			#endif

			Debug.Log ("create screenshot path = " + path);
			
			isChange = true;
		}
	}
}
