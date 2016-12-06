using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleDebris : Obstacle
{
	[SerializeField] private float amountOfSlowdown;
	[SerializeField] private float duration;

	void Start()
	{
		myEffect = new RunnerEffectAdjustMaxSpeed(-amountOfSlowdown, duration);
	}
}