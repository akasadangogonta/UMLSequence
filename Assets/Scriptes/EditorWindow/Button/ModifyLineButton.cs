using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum EditorDirect
{
	Down = -1,
	Up = 1,
}
public enum EditorAct
{
	Remove = -1,
	Add = 1,
}

public delegate void LineEditButtonModify(bool isLineIncreace);

public class ModifyLineButton : AutoAddEventTrigger 
{
	public EditorDirect Direction;
	public EditorAct action;

	public Text nomoreText;
	private Color defaultColor;
	public AnimSupport animSupport;
	private IEnumerator runingCoroutine;

	private LineEditButtonModify ChangePos;

	private EditorTargetControllBase targetObj;

	protected override void Awake ()
	{
		base.Awake ();
		if (nomoreText != null &&
			nomoreText.color.a != 0) 
		{
			nomoreText.color = new Color(0.75F, 0.25F, 0.25F, 0);
			defaultColor = nomoreText.color;

			nomoreText.fontStyle = FontStyle.Italic;
		}
	}

	public void SetData(EditorTargetControllBase baseObj, LineEditButtonModify ChangePos)
	{
		this.targetObj = baseObj;
		this.ChangePos = ChangePos;
	}
	
	public override void OnMouseDown()
	{
		Debug.Log ("linbaBaseObj.type = " + targetObj.type);
		if (targetObj.type == ObjType.Frame) 
		{
			EditorTagetControllOnFrame obj = targetObj.gameObject.GetComponent<EditorTagetControllOnFrame>();

			if(action == EditorAct.Add)
			{
				if(obj.LineNum < 5)
				{
					obj.LineEdit(Direction, action);
					ChangePos(true);
				}
				else
				{
					Debug.Log ("No more Create line");
					ResetAnimation();
					StartCoroutine(NomoreAnimation());
				}
			}
			else if(action == EditorAct.Remove)
			{
				if(obj.LineNum > 1)
				{
					obj.LineEdit(Direction, action);
					ChangePos(false);
				}
				else
				{
					Debug.Log ("No more delete line");
					ResetAnimation();
					StartCoroutine(NomoreAnimation());
				}
			}
		}
	}

	private void ResetAnimation()
	{
		if (runingCoroutine != null)
		{
			StopCoroutine (runingCoroutine);
		}
		StopCoroutine ("NomoreAnimation");
		animSupport.SetColor (nomoreText, defaultColor);
	}

	private IEnumerator NomoreAnimation()
	{
		System.Func<Color> nomoreColor = () => { return nomoreText.color; };
		Color defaultColor = nomoreColor ();

		animSupport.SetColor (nomoreText, 0.9F, 0.15F, 0.15F, 1);

		yield return new WaitForSeconds(0.3F);

		int loopCount;
		float waitSeconds;
		Color newColor;

		loopCount = 3;
		waitSeconds = 0.05F;
		newColor =  animSupport.GetColor (defaultColor, targetColorA: 0);
		runingCoroutine = animSupport.SetColorAnim (nomoreText, loopCount, waitSeconds, newColor);
		yield return StartCoroutine(runingCoroutine);

		animSupport.SetColor (nomoreText, defaultColor);
	}
}
