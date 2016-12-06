using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleRequirementSwipe : PuzzleRequirement
{
	[SerializeField] private bool requireLength;
	[SerializeField] private float requiredLength;
	[SerializeField] private bool requireSlope;
	[SerializeField] private Vector2 requiredSlope;
	[SerializeField] private float slopeMarginOfError;

	Vector2 mouseStartSwipe;
	Vector2 mouseEndSwipe;

	public void TestPrint(string s)
	{
		print(s);
	}

	public void StartSwipe()
	{
		mouseStartSwipe = ConvertPixelPositionToRatioOfScreen(Input.mousePosition);
	}

	public void EndSwipe()
	{
		mouseEndSwipe = ConvertPixelPositionToRatioOfScreen(Input.mousePosition);

		CheckRequirementComplete();
	}

	void CheckRequirementComplete()
	{
		if (requireLength)
		{
			float length = Vector2.Distance(mouseStartSwipe, mouseEndSwipe);
			if (length < requiredLength)
			{
				return;
			}
		}

		if (requireSlope)
		{
			Vector2 slope = (mouseEndSwipe - mouseStartSwipe).normalized;
			float slopeDifference = Vector2.Distance(slope, requiredSlope.normalized);
			if (slopeDifference > slopeMarginOfError)
			{
				return;
			}
		}
		
		ReportRequirementsComplete();
	}
}
