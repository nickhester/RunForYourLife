﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemStaminaAdjust : Item
{
	[SerializeField] private float amount;

	protected override void Start()
	{
		base.Start();

		myEffect = new RunnerEffectStaminaAdjustOnce(amount);
	}
}
