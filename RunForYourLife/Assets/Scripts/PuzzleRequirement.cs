using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PuzzleRequirement : MonoBehaviour
{
	private PuzzlePhase myPhase;

	void Start()
	{
		myPhase = GetComponentInParent<PuzzlePhase>();
	}

	protected void ReportRequirementsComplete()
	{
		myPhase.ReportRequirementsComplete(this);
	}

	public static Vector2 ConvertPixelPositionToRatioOfScreen(Vector2 v)
	{
		v.x = v.x / Screen.width;
		v.y = v.y / Screen.width;	// divide both by width (dividing by height would skew)
		return v;
	}
}
