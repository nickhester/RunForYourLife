using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
	private Item myItem;

	private List<PuzzlePhase> puzzlePhases = new List<PuzzlePhase>();

	void Start ()
	{
		puzzlePhases.AddRange(GetComponentsInChildren<PuzzlePhase>());
		puzzlePhases.Sort();
		SetNextPhaseUp();
	}

	public void Initialize(Item item)
	{
		myItem = item;
	}

	void SetNextPhaseUp()
	{
		if (puzzlePhases.Count > 0)
		{
			int phaseOrderNumber = puzzlePhases[0].order;
			for (int i = 0; i < puzzlePhases.Count; i++)
			{
				// set first one active, and all others inactive
				puzzlePhases[i].gameObject.SetActive(puzzlePhases[i].order == phaseOrderNumber);
			}
		}
		
	}

	public void ReportPhaseComplete(PuzzlePhase phase)
	{
		puzzlePhases.Remove(phase);
		Destroy(phase.gameObject);

		if (puzzlePhases.Count == 0)
		{
			PuzzleCompleted();
		}
		else
		{
			SetNextPhaseUp();
		}
	}

	public void ReportPuzzleDropped()
	{
		myItem.ReportPuzzleDropped();
		Destroy(gameObject);
	}

	void PuzzleCompleted()
	{
		myItem.ReportPuzzleCompleted();
		Destroy(gameObject);
	}
}
