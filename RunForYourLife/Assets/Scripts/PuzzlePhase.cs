using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzlePhase : MonoBehaviour, IComparable
{
	List<PuzzleRequirement> requirements = new List<PuzzleRequirement>();
	Puzzle myPuzzle;
	public int order;
	
	void Start ()
	{
		myPuzzle = GetComponentInParent<Puzzle>();
		requirements.AddRange(GetComponentsInChildren<PuzzleRequirement>());

		if (requirements.Count == 0)
		{
			Debug.LogWarning("puzzle phase has no requirements");
		}
	}

	public void ReportRequirementsComplete(PuzzleRequirement requirement)
	{
		requirements.Remove(requirement);

		if (requirements.Count == 0)
		{
			myPuzzle.ReportPhaseComplete(this);
		}
	}

	public int CompareTo(object obj)
	{
		PuzzlePhase otherPuzzle = obj as PuzzlePhase;
		if (order < otherPuzzle.order)
		{
			return -1;
		}
		return 1;
	}
}
