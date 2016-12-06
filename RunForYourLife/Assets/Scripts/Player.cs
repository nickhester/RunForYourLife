﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Player : Runner
{
	// singleton
	private static Player instance;
	// instance
	public static Player Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(Player)) as Player;
			}
			return instance;
		}
	}

	[SerializeField] private float footstepPrimeSpeedUpAmount;
	[SerializeField] private float footstepActivateSpeedUpAmount;
	[SerializeField] private float footstepPrimeJumpToAtLeastAmount;
	[SerializeField] private float missedStepNewSpeed;
	[SerializeField] private float stumbleMinSpeed;
	private Animator animator;
	private bool playerHasBeenCaught = false;
	private float mouseHoldTime = 0.0f;
	[SerializeField] private float mouseHoldTimeToStop;
	[SerializeField] private float mouseHoldStopAmount;

	private float currentStamina;
	[SerializeField] float startingStamina;
	[SerializeField] float staminaMax;
	[SerializeField] float staminaUsageRate;
	[SerializeField] float staminaRegenRate;
	[SerializeField] float speedToSpendStamina;
	private Slider sliderStamina;
	
	private List<Item> inventory = new List<Item>();
	private Item itemInHand = null;

	[SerializeField] private Text distanceDisplay;
	private float distanceRun;
	private float startZValue;
	
	public bool debug_noStumble = false;
	public bool debug_noDie = false;
	public bool debug_alwaysRun = false;

	protected override void Start()
	{
		base.Start();

		currentMoveSpeed = startMoveSpeed;
		animator = GetComponentInChildren<Animator>();
		currentStamina = startingStamina;
		sliderStamina = GetComponentInChildren<Slider>();

		startZValue = transform.position.z;
		distanceRun = transform.position.z - startZValue;
		distanceDisplay.text = distanceRun.ToString();
	}
	
	protected override void Update ()
	{
		base.Update();

		if (currentMoveSpeed > speedToSpendStamina)
		{
			currentStamina = Mathf.Max(currentStamina - staminaUsageRate * Time.deltaTime, 0.0f);
			if (currentStamina <= 0.0f)
			{
				FindObjectOfType<FootprintManager>().FootprintMissed(null);
			}
		}
		else
		{
			currentStamina = Mathf.Min(currentStamina + staminaRegenRate * Time.deltaTime, staminaMax);
		}
		sliderStamina.value = currentStamina / staminaMax;

		if (Input.GetMouseButton(0))
		{
			mouseHoldTime += Time.deltaTime;
			if (mouseHoldTime > mouseHoldTimeToStop)
			{
				currentMoveSpeed -= mouseHoldStopAmount;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			mouseHoldTime = 0.0f;
		}

		// update distance display
		distanceRun = transform.position.z - startZValue;
		distanceDisplay.text = (int)distanceRun + " meters";

		if (debug_alwaysRun)
		{
			currentMoveSpeed = maxMoveSpeed;
			currentStamina = staminaMax;
		}
	}

	public void ReportFootprintSelected(System.Type footprintState)
	{
		if (footprintState == typeof(FootprintStatePrime))
		{
			currentMoveSpeed = Mathf.Clamp(currentMoveSpeed + footstepPrimeSpeedUpAmount, footstepPrimeJumpToAtLeastAmount, maxMoveSpeed);
		}
		else if (footprintState == typeof(FootprintStateActive))
		{
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + footstepActivateSpeedUpAmount, maxMoveSpeed);
		}
	}

	public void ReportFootprintMissed()
	{
		if (!debug_noStumble && currentMoveSpeed > stumbleMinSpeed)
		{
			animator.SetTrigger("stumble");

			currentMoveSpeed = missedStepNewSpeed;
		}
	}

	public void ReportPursuerCaughtPlayer()
	{
		if (!debug_noDie && GetRunnerIsStillRunning())
		{
			animator.SetTrigger("getCaught");
			playerHasBeenCaught = true;

			Invoke("ReloadScene", 5.0f);
		}
	}

	void ReloadScene()
	{
		SceneManager.LoadScene(0);
	}

	public override bool GetRunnerIsStillRunning()
	{
		return !playerHasBeenCaught;
	}

	public void AddItemToInventory(Item item)
	{
		inventory.Add(item);
	}

	protected override void ExecuteOneTimeEffects()
	{
		inventory.Add(currentRunnerEffect.itemToAddToInventory);
		currentMoveSpeed += currentRunnerEffect.oneTimeSpeedAdjustment;
		currentStamina += currentRunnerEffect.oneTimeStaminaLevelAdjustment;
	}

	public bool isPlayerHandAvailable()
	{
		return (itemInHand == null);
	}

	public void AddItemToHand(Item item)
	{
		itemInHand = item;
	}

	public void RemoveItemFromHand(Item item)
	{
		if (item != itemInHand) { Debug.LogWarning("removing item from hand, but other item was found in player hand. ");  }
		itemInHand = null;
	}
}
