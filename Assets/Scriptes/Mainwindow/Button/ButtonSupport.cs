using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonSupport : AutoAddEventTrigger
{
	Button button;
	ColorBlock defaultColor;
	ColorBlock newColor;
	bool switchOK = true;

	Color highlightedColor = new Color32(255, 253, 253, 255);

	protected float GetScale { get { return GeneralController.GetScale; } }

	private bool _CheckColor()
	{
		if (defaultColor.normalColor != button.colors.normalColor) {
			return false;
		}
		else
		{
			return true;
		}
	}

	private void SetColor()
	{
		if (button != null)
		{
			defaultColor = button.colors;
			newColor = defaultColor;
			newColor.highlightedColor = this.highlightedColor;
		}
	}

	override protected void Awake()
	{
		base.Awake ();

		button = GetComponent<Button> ();
		SetColor ();
	}
	

	override public void OnMouseDown()
	{
		if (button == null) return;
		switchOK = false;
		button.colors = defaultColor;
	}

	override public void OnEnter()
	{
		if (!_CheckColor())
		{
			SetColor();
		}

		if (button == null) return;
		if (switchOK == true)
		{
			button.colors = newColor;
		}
	}

	override public void OnExit()
	{
		if (button == null) return;
		switchOK = true;

		button.colors = defaultColor;

		/*
		if (button.colors.highlightedColor == this.highlightedColor) 
		{
			StartCoroutine(AutoEraseColor());
		}*/
	}

	private IEnumerator AutoEraseColor()
	{
		yield return new WaitForSeconds (0.5F);
		if (button.colors.highlightedColor == this.highlightedColor) 
		{
			button.colors = defaultColor;
		}
	}

}
