using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
	private List<Button> puzzleButtons = new List<Button>();
	private Item myItem;

	void Start ()
	{
		puzzleButtons.AddRange(GetComponentsInChildren<Button>());
	}
	
	void Update ()
	{
		
	}

	public void Initialize(Item item)
	{
		myItem = item;
	}

	public void ButtonChosen(Button button)
	{
		if (puzzleButtons.Contains(button))
		{
			button.interactable = false;
		}

		if (IsPuzzleComplete())
		{
			PuzzleCompleted();
		}
	}

	void PuzzleCompleted()
	{
		myItem.ReportPuzzleCompleted();
		Destroy(gameObject);
	}

	bool IsPuzzleComplete()
	{
		for (int i = 0; i < puzzleButtons.Count; i++)
		{
			if (puzzleButtons[i].IsInteractable())
			{
				return false;
			}
		}
		return true;
	}
}
