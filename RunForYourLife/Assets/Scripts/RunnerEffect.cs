using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class RunnerEffect
{
	// one time effects
	public float oneTimeSpeedAdjustment = 0.0f;
	public float oneTimeStaminaLevelAdjustment = 0.0f;

	// temporary on going effects
	public float temporaryMaxSpeedAdjustment = 0.0f;
	public float temporaryStaminaRegenIncrease = 0.0f;
	public bool temporaryPlayerCantStumble = false;
	public bool temporaryPlayerNoStaminaLoss = false;

	public float temporaryTimeLeft = 0.0f;

	public Item itemToAddToInventory = null;
}

public class RunnerEffectNoEffect : RunnerEffect
{
	public RunnerEffectNoEffect()
	{
		
	}
}

public class RunnerEffectSpeedOnce : RunnerEffect
{
	public RunnerEffectSpeedOnce(float v)
	{
		oneTimeSpeedAdjustment = v;
	}
}

public class RunnerEffectAdjustMaxSpeed : RunnerEffect
{
	public RunnerEffectAdjustMaxSpeed(float v, float t)
	{
		temporaryMaxSpeedAdjustment = v;
		temporaryTimeLeft = t;
	}
}

public class RunnerEffectStaminaAdjustOnce : RunnerEffect
{
	public RunnerEffectStaminaAdjustOnce(float v)
	{
		oneTimeStaminaLevelAdjustment = v;
	}
}

public class RunnerEffectAdjustStaminaRegen : RunnerEffect
{
	public RunnerEffectAdjustStaminaRegen(float v, float t)
	{
		temporaryStaminaRegenIncrease = v;
		temporaryTimeLeft = t;
	}
}

public class RunnerEffectMaxSpeedAndNoStumbleAndNoStaminaUse : RunnerEffect
{
	public RunnerEffectMaxSpeedAndNoStumbleAndNoStaminaUse(float v, float t)
	{
		temporaryMaxSpeedAdjustment = v;
		temporaryTimeLeft = t;
		temporaryPlayerCantStumble = true;
		temporaryPlayerNoStaminaLoss = true;
	}
}

