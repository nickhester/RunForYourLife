using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Footprint : MonoBehaviour
{
	private FootprintManager footprintManager;
	private FootprintState myFootprintStateObject;

	void Start()
	{
		myFootprintStateObject = new FootprintStateInactive();
		footprintManager = FindObjectOfType<FootprintManager>();
		footprintManager.RegisterFootprint(this);
	}

	void Update()
	{
		float distanceFromPlayer = footprintManager.GetDistanceToPlayer(transform.position);
		FootprintState newState = myFootprintStateObject.CheckChangeConditions(distanceFromPlayer, footprintManager);
		if (newState != null)
		{
			myFootprintStateObject = newState;
			myFootprintStateObject.SetFootprintState(GetComponentInChildren<Renderer>().material, footprintManager);
		}
		
		if (myFootprintStateObject.GetType() == typeof(FootprintStateActive))
		{
			if (distanceFromPlayer < footprintManager.GetDistanceToMissStep())
			{
				footprintManager.FootprintMissed(this);
			}
		}
	}

	void OnMouseDown()
	{
		SelectFootprint();
	}

	void SelectFootprint()
	{
		if (myFootprintStateObject.GetType() == typeof(FootprintStateActive) || myFootprintStateObject.GetType() == typeof(FootprintStatePrime))
		{
			footprintManager.PlayerSelectedFootprint(this);
		}
	}

	public bool GetCanBeSelected()
	{
		return myFootprintStateObject.GetType() == typeof(FootprintStateActive) || myFootprintStateObject.GetType() == typeof(FootprintStatePrime);
	}

	public void SetAsUpNext()
	{
		myFootprintStateObject = new FootprintStateReady();
		myFootprintStateObject.SetFootprintState(GetComponentInChildren<Renderer>().material, footprintManager);
	}

	public FootprintState GetMyFootprintStateObject()
	{
		return myFootprintStateObject;
	}
}






public interface FootprintState
{
	FootprintState CheckChangeConditions(float distanceFromPlayer, FootprintManager fm);

	void SetFootprintState(Material m, FootprintManager fm);
}

public class FootprintStateInactive : FootprintState
{
	public FootprintState CheckChangeConditions(float distanceFromPlayer, FootprintManager fm)
	{
		// doesn't change itself
		return null;
	}

	public void SetFootprintState(Material m, FootprintManager fm)
	{
		m.SetTexture("_MainTex", fm.inactiveFootPrint);
	}
}

public class FootprintStateReady : FootprintState
{
	public FootprintState CheckChangeConditions(float distanceFromPlayer, FootprintManager fm)
	{
		if (distanceFromPlayer < fm.GetDistanceToActivatePrime())
		{
			return new FootprintStatePrime();
		}
		return null;
	}

	public void SetFootprintState(Material m, FootprintManager fm)
	{
		// no change
	}
}

public class FootprintStatePrime : FootprintState
{
	public FootprintState CheckChangeConditions(float distanceFromPlayer, FootprintManager fm)
	{
		if (distanceFromPlayer < fm.GetDistanceToActivateNormal())
		{
			return new FootprintStateActive();
		}
		return null;
	}

	public void SetFootprintState(Material m, FootprintManager fm)
	{
		m.SetTexture("_MainTex", fm.primeFootPrint);
	}
}

public class FootprintStateActive : FootprintState
{
	public FootprintState CheckChangeConditions(float distanceFromPlayer, FootprintManager fm)
	{
		return null;
	}

	public void SetFootprintState(Material m, FootprintManager fm)
	{
		m.SetTexture("_MainTex", fm.activeFootPrint);
	}
}