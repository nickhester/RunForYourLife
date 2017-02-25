using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleRequirementSwipe : PuzzleRequirement
{
	[SerializeField] private bool requireLength;
	[SerializeField] private float requiredLength;
	[SerializeField] private bool requireSlope;
	[SerializeField] private Vector2 requiredSlope;
	private float slopeMarginOfError = 35.0f;	// angle in degrees

	Vector2 mouseStartSwipe;
	Vector2 mouseEndSwipe;

	public void StartSwipe()
	{
		mouseStartSwipe = ConvertPixelPositionToRatioOfScreen(Input.mousePosition);
	}

	public void EndSwipe()
	{
		mouseEndSwipe = ConvertPixelPositionToRatioOfScreen(Input.mousePosition);

		if (CheckRequirementComplete())
		{
			ReportRequirementsComplete();
		}
	}

	bool CheckRequirementComplete()
	{
		if (requireLength)
		{
			float length = Vector2.Distance(mouseStartSwipe, mouseEndSwipe);
			if (length < requiredLength)
			{
				return false;
			}
		}

		if (requireSlope)
		{
			Vector2 slope = (mouseEndSwipe - mouseStartSwipe);
			//float slopeDifference = Vector2.Distance(slope.normalized, requiredSlope.normalized);
			float slopeDifference = Vector2.Angle(slope.normalized, requiredSlope.normalized);
			if (Mathf.Abs(slopeDifference) > slopeMarginOfError)
			{
				return false;
			}
		}
		return true;
	}
}
