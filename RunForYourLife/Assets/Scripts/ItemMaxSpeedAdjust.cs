using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMaxSpeedAdjust : Item
{
	[SerializeField] private float amount;
	[SerializeField] private float duration;

	protected override void Start()
	{
		base.Start();

		myEffect = new RunnerEffectMaxSpeedAndNoStumbleAndNoStaminaUse(amount, duration);
	}
}
