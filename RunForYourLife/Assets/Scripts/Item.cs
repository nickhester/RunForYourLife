using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
	[SerializeField] private Puzzle myPuzzlePrefab;
	protected Puzzle myPuzzle;
	protected RunnerEffect myEffect;

	[SerializeField] private RectTransform uiCenterIcon;
	private RectTransform canvasRect;
	private bool hasBeenPickedUp = false;

	protected virtual void Start()
	{
		canvasRect = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
		MoveUIOverObject();
	}

	void Update()
	{
		MoveUIOverObject();

		// if it hasn't been picked up, and the player has passed it, destroy it
		if (!hasBeenPickedUp && Player.Instance.transform.position.z > transform.position.z)
		{
			Destroy(gameObject);
		}
	}

	void MoveUIOverObject()
	{
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
		Vector2 WorldObject_ScreenPosition = new Vector2(
		((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
		((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

		uiCenterIcon.anchoredPosition = WorldObject_ScreenPosition;
	}

	public void PickUpItem()
	{
		if (Player.Instance.isPlayerHandAvailable())
		{
			hasBeenPickedUp = true;
			MakeInvisible();
			Player.Instance.AddItemToHand(this);

			// activate puzzle
			myPuzzle = Instantiate(myPuzzlePrefab) as Puzzle;
			myPuzzle.Initialize(this);
		}
	}

	void MakeInvisible()
	{
		// disappear
		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		canvasRect.gameObject.SetActive(false);
	}

	public void ReportPuzzleCompleted()
	{
		// apply effect to player
		Runner runner = Player.Instance as Runner;
		runner.ApplyEffect(myEffect);

		Player.Instance.RemoveItemFromHand(this);
	}
}
