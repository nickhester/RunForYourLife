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
	private float currentStaminaRegenRate;
	[SerializeField] float speedToSpendStamina;
	private Slider sliderStamina;
	[SerializeField] private Image sliderColor;
	[SerializeField] float staminaLevelLow;
	[SerializeField] float staminaLevelHigh;
	[SerializeField] float staminaLowDecreasedLevel;
	[SerializeField] float staminaHighIncreasedLevel;
	enum StaminaLevel
	{
		LOW,
		MID,
		HIGH,
		SPECIAL
	}

	private bool cantStumble = false;
	private bool noStaminaLoss = false;
	
	private List<Item> inventory = new List<Item>();
	private Item itemInHand = null;

	[SerializeField] private Text distanceDisplay;
	private float distanceRun;
	private float startZValue;
	
	public bool debug_noStumble = false;
	public bool debug_noDie = false;
	public bool debug_alwaysRun = false;
	public bool debug_infiniteStamina = false;

	protected override void Start()
	{
		base.Start();
		
		animator = GetComponentInChildren<Animator>();
		currentStamina = startingStamina;
		currentStaminaRegenRate = staminaRegenRate;
		sliderStamina = GetComponentInChildren<Slider>();

		cantStumble = debug_noStumble;

		startZValue = transform.position.z;
		distanceRun = transform.position.z - startZValue;
		distanceDisplay.text = distanceRun.ToString();
	}
	
	protected override void Update ()
	{
		// set these before base update, so base update can calculate temp effects
		currentStaminaRegenRate = staminaRegenRate;
		cantStumble = debug_noStumble;
		noStaminaLoss = false;

		base.Update();

		// calculate stamina
		StaminaLevel currentStaminaLevel = StaminaLevel.SPECIAL;
		if (!noStaminaLoss)
		{
			// check if regening or using stamina
			if (currentMoveSpeed > speedToSpendStamina)
			{
				currentStamina = Mathf.Max(currentStamina - staminaUsageRate * Time.deltaTime, 0.0f);
			}
			else
			{
				currentStamina = Mathf.Min(currentStamina + currentStaminaRegenRate * Time.deltaTime, staminaMax);
			}

			// check stamina level
			if (currentStamina <= staminaLevelLow)
			{
				currentMaxMoveSpeed *= staminaLowDecreasedLevel;
				currentStaminaLevel = StaminaLevel.LOW;
			}
			else if (currentStamina >= staminaLevelHigh)
			{
				currentMaxMoveSpeed *= staminaHighIncreasedLevel;
				currentStaminaLevel = StaminaLevel.HIGH;
			}
			else
			{
				currentStaminaLevel = StaminaLevel.MID;
			}
		}
		
		sliderStamina.value = currentStamina / staminaMax;
		ColorSlider(currentStaminaLevel);

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
			currentMoveSpeed = currentMaxMoveSpeed;
		}
		if (debug_infiniteStamina)
		{
			currentStamina = staminaMax;
		}

		// DEBUG!
		if (Input.GetKeyDown(KeyCode.Space))
		{
			debug_noStumble = true;
			Debug_KeyboardStep();
		}
	}

	private void ColorSlider(StaminaLevel s)
	{
		switch (s)
		{
			case StaminaLevel.LOW:
				sliderColor.color = Color.red;
				break;
			case StaminaLevel.MID:
				sliderColor.color = Color.blue;
				break;
			case StaminaLevel.HIGH:
				sliderColor.color = Color.green;
				break;
			case StaminaLevel.SPECIAL:
				sliderColor.color = Color.white;
				break;
			default:
				break;
		}

		//sliderColor.color = 
	}

	public void ReportFootprintSelected(System.Type footprintState)
	{
		if (footprintState == typeof(FootprintStatePrime))
		{
			currentMoveSpeed = Mathf.Clamp(currentMoveSpeed + footstepPrimeSpeedUpAmount, footstepPrimeJumpToAtLeastAmount, currentMaxMoveSpeed);
		}
		else if (footprintState == typeof(FootprintStateActive))
		{
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + footstepActivateSpeedUpAmount, currentMaxMoveSpeed);
		}
	}

	public void ReportFootprintMissed()
	{
		if (!cantStumble && currentMoveSpeed > stumbleMinSpeed)
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

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("DebugMainMenu");
	}

	public override bool GetRunnerIsStillRunning()
	{
		return !playerHasBeenCaught;
	}

	public void AddItemToInventory(Item item)
	{
		inventory.Add(item);
	}

	protected override void ExecuteOneTimeEffects(RunnerEffect effect)
	{
		inventory.Add(effect.itemToAddToInventory);
		currentMoveSpeed += effect.oneTimeSpeedAdjustment;
		currentStamina += effect.oneTimeStaminaLevelAdjustment;
	}

	protected override void ApplyPersistentEffects(RunnerEffect effect)
	{
		currentMaxMoveSpeed += effect.temporaryMaxSpeedAdjustment;
		currentStaminaRegenRate += effect.temporaryStaminaRegenIncrease;
		if (effect.temporaryPlayerCantStumble)
		{
			cantStumble = true;
		}
		if (effect.temporaryPlayerNoStaminaLoss)
		{
			noStaminaLoss = true;
		}
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

	private void Debug_KeyboardStep()
	{
		ReportFootprintSelected(typeof(FootprintStatePrime));
	}
}
