using UnityEngine;
using System.Collections;

public enum PartsOfFrame
{
	Button = 0,
	LineText,
	InputField,
}

public class GlobalConfig : MonoBehaviour
{
	public static readonly float oneLineDistanceOfFrame = 33.0F;
	public static readonly float oneLineDistanceOfText = 25.0F;

	public static readonly int defaultFrameLineNum = 2;

}
