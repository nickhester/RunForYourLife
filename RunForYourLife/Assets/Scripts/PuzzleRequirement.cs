﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class PuzzleRequirement : MonoBehaviour
{
	private PuzzlePhase myPhase;
	
	[SerializeField] protected Image targetImage;
	[SerializeField] protected bool removeWhenCompleted = true;
	[SerializeField] protected bool updateImageWhenCompleted = false;
	[SerializeField] protected Sprite completedImage;

	void Start()
	{
		myPhase = GetComponentInParent<PuzzlePhase>();
	}

	protected void ReportRequirementsComplete()
	{
		myPhase.ReportRequirementsComplete(this);

		if (removeWhenCompleted)
		{
			if (targetImage != null)
			{
				Destroy(targetImage.gameObject);
			}
			Destroy(gameObject);
			return;
		}

		if (updateImageWhenCompleted && completedImage != null)
		{
			targetImage.sprite = completedImage;
		}
	}

	public static Vector2 ConvertPixelPositionToRatioOfScreen(Vector2 v)
	{
		v.x = v.x / Screen.width;
		v.y = v.y / Screen.width;	// divide both by width (dividing by height would skew)
		return v;
	}
}
