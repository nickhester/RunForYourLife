using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Pursuer : Runner, IEventSubscriber
{
	private Slider sliderDistanceFromPlayer;
	private Canvas myCanvas;
	[SerializeField] private float distanceFromPlayerToStartSliderProgress;
	[SerializeField] private float checkTakeABreakInterval;
	private float checkTakeABreakCounter;
	[SerializeField] private float startBreakLikelihoodPerInterval;
	[SerializeField] private float endBreakLikelihoodPerInterval;
	private bool isTakingABreak = false;
	private List<Obstacle> obstaclesAhead = new List<Obstacle>();

	protected override void Start ()
	{
		base.Start();
		
		sliderDistanceFromPlayer = GetComponentInChildren<Slider>();
		myCanvas = GetComponentInChildren<Canvas>();
		myCanvas.transform.SetParent(null);
		myCanvas.transform.position = Vector3.zero;

		UpdateObstacleList();
		GameObject.FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.OBSTACLE_UPDATED, this);
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

		// check if hitting obstacle
		for (int i = 0; i < obstaclesAhead.Count; i++)
		{
			if (obstaclesAhead[i] == null)
			{
				obstaclesAhead.RemoveAt(i);
			}
			else if (obstaclesAhead[i].gameObject.transform.position.z < transform.position.z)
			{
				HitObstacle(obstaclesAhead[i]);
			}
		}
	}

	void HitObstacle(Obstacle o)
	{
		print("I'm hitting an obstacle");
		ApplyEffect(o.myEffect);
		Destroy(o.gameObject);
		FindObjectOfType<EventBroadcast>().TriggerEvent(EventBroadcast.Event.OBSTACLE_UPDATED);
	}

	void OnDestroy()
	{
		Destroy(myCanvas);
	}

	void TakeABreak(bool b)
	{
		isTakingABreak = b;
	}

	public override bool GetRunnerIsStillRunning()
	{
		return true;
	}

	protected override void ExecuteOneTimeEffects(RunnerEffect effect)
	{
		currentMoveSpeed += effect.oneTimeSpeedAdjustment;
	}

	protected override void ApplyPersistentEffects(RunnerEffect effect)
	{
		currentMaxMoveSpeed += effect.temporaryMaxSpeedAdjustment;
	}

	void UpdateObstacleList()
	{
		List<Obstacle> allObstacles = new List<Obstacle>();
		allObstacles.AddRange(FindObjectsOfType<Obstacle>());
		for (int i = 0; i < allObstacles.Count; i++)
		{
			if (!obstaclesAhead.Contains(allObstacles[i]) && allObstacles[i].gameObject.transform.position.z > transform.position.z)
			{
				obstaclesAhead.Add(allObstacles[i]);
			}
		}
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.OBSTACLE_UPDATED)
		{
			UpdateObstacleList();
		}
	}
}
