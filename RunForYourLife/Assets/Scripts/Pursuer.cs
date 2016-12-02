using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Pursuer : Runner
{
	private Slider sliderDistanceFromPlayer;
	private Canvas myCanvas;
	[SerializeField] private float distanceFromPlayerToStartSliderProgress;
	[SerializeField] private float checkTakeABreakInterval;
	private float checkTakeABreakCounter;
	[SerializeField] private float startBreakLikelihoodPerInterval;
	[SerializeField] private float endBreakLikelihoodPerInterval;
	private bool isTakingABreak = false;

	protected override void Start ()
	{
		base.Start();

		currentMoveSpeed = startMoveSpeed;
		sliderDistanceFromPlayer = GetComponentInChildren<Slider>();
		myCanvas = GetComponentInChildren<Canvas>();
		myCanvas.transform.SetParent(null);
		myCanvas.transform.position = Vector3.zero;
	}
	
	protected override void Update ()
	{
		base.Update();

		float distanceFromPlayer = Player.Instance.transform.position.z - transform.position.z;
		float slidervalue = Mathf.Clamp01(-(distanceFromPlayer / distanceFromPlayerToStartSliderProgress) + 1.0f);
		sliderDistanceFromPlayer.value = slidervalue;

		myCanvas.gameObject.SetActive(slidervalue > 0.0f);

		if (distanceFromPlayer <= 0.0f)
		{
			sliderDistanceFromPlayer.gameObject.SetActive(false);
			Player.Instance.ReportPursuerCaughtPlayer();
		}

		checkTakeABreakCounter += Time.deltaTime;
		if (checkTakeABreakCounter > checkTakeABreakInterval)
		{
			checkTakeABreakCounter = 0.0f;

			float randValue = UnityEngine.Random.Range(0.0f, 1.0f);
			float chance = (isTakingABreak ? endBreakLikelihoodPerInterval : startBreakLikelihoodPerInterval);
			if (randValue <= chance)
			{
				TakeABreak(!isTakingABreak);
			}
		}

		if (isTakingABreak)
		{
			currentMoveSpeed = minMoveSpeed;
		}
	}

	void OnDestroy()
	{
		Destroy(myCanvas);
	}

	void TakeABreak(bool b)
	{
		print("toggling break");
		isTakingABreak = b;
	}

	public override bool GetRunnerIsStillRunning()
	{
		return true;
	}

	protected override void ExecuteOneTimeEffects()
	{
		currentMoveSpeed += currentRunnerEffect.oneTimeSpeedAdjustment;
	}
}
