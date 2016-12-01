using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] private float startMoveSpeed;
	[SerializeField] private float minMoveSpeed;
	[SerializeField] private float maxMoveSpeed;
	private float currentMoveSpeed;
	[SerializeField] private float moveSlowDownRate;
	[SerializeField] private float footstepPrimeSpeedUpAmount;
	[SerializeField] private float footstepActivateSpeedUpAmount;
	[SerializeField] private float missedStepNewSpeed;
	private Animator animator;
	private bool playerHasBeenCaught = false;
	
	private float currentStamina;
	[SerializeField] float startingStamina;
	[SerializeField] float staminaMax;
	[SerializeField] float staminaUsageRate;
	[SerializeField] float staminaRegenRate;
	[SerializeField] float speedToSpendStamina;
	private Slider sliderStamina;
	
	public bool debug_noStumble = false;
	public bool debug_noDie = false;

	void Start ()
	{
		currentMoveSpeed = startMoveSpeed;
		animator = GetComponentInChildren<Animator>();
		currentStamina = startingStamina;
		sliderStamina = GetComponentInChildren<Slider>();
	}
	
	void Update ()
	{
		if (!GetPlayerHasBeenCaught())
		{
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

			currentMoveSpeed = Mathf.Max(currentMoveSpeed - moveSlowDownRate * Time.deltaTime, minMoveSpeed);

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
		}
	}

	public void ReportFootprintSelected(System.Type footprintState)
	{
		if (footprintState == typeof(FootprintStatePrime))
		{
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + footstepPrimeSpeedUpAmount, maxMoveSpeed);
		}
		else if (footprintState == typeof(FootprintStateActive))
		{
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + footstepActivateSpeedUpAmount, maxMoveSpeed);
		}
	}

	public void ReportFootprintMissed()
	{
		if (!debug_noStumble)
		{
			animator.SetTrigger("stumble");

			currentMoveSpeed = missedStepNewSpeed;
		}
	}

	public void ReportPursuerCaughtPlayer()
	{
		if (!debug_noDie)
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

	public bool GetPlayerHasBeenCaught()
	{
		return playerHasBeenCaught;
	}
}
