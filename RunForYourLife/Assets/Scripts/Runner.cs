using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Runner : MonoBehaviour
{
	[SerializeField] protected float startMoveSpeed;
	[SerializeField] protected float minMoveSpeed;
	[SerializeField] protected float maxMoveSpeed;
	protected float currentMaxMoveSpeed;
	[SerializeField] protected float moveGradualAdjustmentRate;
	protected float currentMoveSpeed;
	protected List<RunnerEffect> currentRunnerEffects = new List<RunnerEffect>();

	protected virtual void Start()
	{
		currentMoveSpeed = startMoveSpeed;
		currentMaxMoveSpeed = maxMoveSpeed;
	}

	protected virtual void Update()
	{
		// keep setting this b/c persistent effects may set these in their update
		currentMaxMoveSpeed = maxMoveSpeed;
		// then apply persistent effects
		for (int i = 0; i < currentRunnerEffects.Count; i++)
		{
			currentRunnerEffects[i].temporaryTimeLeft -= Time.deltaTime;
			if (currentRunnerEffects[i].temporaryTimeLeft < 0.0f)
			{
				currentRunnerEffects.RemoveAt(i);
				i--;
			}
			else
			{
				ApplyPersistentEffects(currentRunnerEffects[i]);
			}
		}

		if (GetRunnerIsStillRunning())
		{
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

			currentMoveSpeed = Mathf.Clamp(currentMoveSpeed + moveGradualAdjustmentRate * Time.deltaTime, minMoveSpeed, currentMaxMoveSpeed);
		}
	}

	public abstract bool GetRunnerIsStillRunning();

	public void ApplyEffect(RunnerEffect effect)
	{
		currentRunnerEffects.Add(effect);
		ExecuteOneTimeEffects(effect);
	}

	protected abstract void ExecuteOneTimeEffects(RunnerEffect effect);

	protected abstract void ApplyPersistentEffects(RunnerEffect effect);
}
