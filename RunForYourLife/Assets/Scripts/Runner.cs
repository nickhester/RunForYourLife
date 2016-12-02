using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Runner : MonoBehaviour
{
	[SerializeField] protected float startMoveSpeed;
	[SerializeField] protected float minMoveSpeed;
	[SerializeField] protected float maxMoveSpeed;
	[SerializeField] protected float moveGradualAdjustmentRate;
	protected float currentMoveSpeed;
	protected RunnerEffect currentRunnerEffect;

	protected virtual void Start()
	{
		currentRunnerEffect = new RunnerEffectNoEffect();
	}

	protected virtual void Update()
	{
		if (GetRunnerIsStillRunning())
		{
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

			currentMoveSpeed = Mathf.Clamp(currentMoveSpeed + moveGradualAdjustmentRate * Time.deltaTime, minMoveSpeed, maxMoveSpeed);
		}
	}

	public abstract bool GetRunnerIsStillRunning();

	public void ApplyEffect(RunnerEffect effect)
	{
		currentRunnerEffect = effect;
		ExecuteOneTimeEffects();
	}

	protected abstract void ExecuteOneTimeEffects();
}
