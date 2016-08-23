using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationSupport : MonoBehaviour 
{
	public Color GetColor(Color color, float targetColorR = -1, float targetColorG = -1, float targetColorB = -1, float targetColorA = -1)
	{
		if (targetColorR == -1) targetColorR = color.r;
		if (targetColorG == -1) targetColorG = color.g;
		if (targetColorB == -1) targetColorB = color.b;
		if (targetColorA == -1) targetColorA = color.a;

		Color newColor = new Color (targetColorR, targetColorG, targetColorB, targetColorA);
		return newColor;
	}

	public void SetColor(Text text, Color targetColor)
	{
		text.color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a);
	}

	public void SetColor(Text text, float targetColorR = -1, float targetColorG = -1, float targetColorB = -1, float targetColorA = -1)
	{
		if (targetColorR == -1) targetColorR = text.color.r;
		if (targetColorG == -1) targetColorG = text.color.g;
		if (targetColorB == -1) targetColorB = text.color.b;
		if (targetColorA == -1) targetColorA = text.color.a;

		text.color = new Color(targetColorR, 
		                       targetColorG,
		                       targetColorB,
		                       targetColorA);
	}

	public IEnumerator SetColorAnim(Text text, int loopCount = 1, float waitSeconds = -1,
	                                float targetColorR = -1, 
	                                float targetColorG = -1, 
	                                float targetColorB = -1,
	                                float targetColorA = -1)
	{
		if (targetColorR == -1) targetColorR = text.color.r;
		if (targetColorG == -1) targetColorG = text.color.g;
		if (targetColorB == -1) targetColorB = text.color.b;
		if (targetColorA == -1) targetColorA = text.color.a;
		Color targetColor = new Color (targetColorR, targetColorG, targetColorB, targetColorA);
	
		yield return StartCoroutine(_SetColorAnimation(text, loopCount, waitSeconds, targetColor));
	}
	
	public IEnumerator SetColorAnim(Text text, int loopCount, float waitSeconds, Color targetColor)
	{
		yield return StartCoroutine(_SetColorAnimation(text, loopCount, waitSeconds, targetColor));
	}

	private IEnumerator _SetColorAnimation(Text text, int loopCount, float waitSeconds, Color targetColor)
	{
		System.Func<Color> funcColor = () => { return text.color; };

		float rValue = (targetColor.r - funcColor().r) / loopCount;
		float gValue = (targetColor.g - funcColor().g) / loopCount;
		float bValue = (targetColor.b - funcColor().b) / loopCount;
		float aValue = (targetColor.a - funcColor().a) / loopCount;

		for(int count = 0; count < loopCount; count++)
		{
			text.color = new Color(funcColor().r + rValue, 
			                       funcColor().g + gValue, 
			                       funcColor().b + bValue, 
			                       funcColor().a + aValue);	

			if(waitSeconds != -1)
			{
				yield return new WaitForSeconds(waitSeconds);
			}
		}
	}
}
