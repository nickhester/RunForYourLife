using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FootprintManager : MonoBehaviour
{

	private Player player;
	private List<FootprintRegister> existingFootprints = new List<FootprintRegister>();

	[SerializeField] private float distanceToActivateNormal;
	[SerializeField] private float distanceToActivatePrime;
	[SerializeField] private float distanceToMissStep;
	[SerializeField] private float distanceToRemoveFootprintsAfterMissStep;
	[SerializeField] private float delayAfterMissStep;
	
	public Texture inactiveFootPrint;
	public Texture activeFootPrint;
	public Texture primeFootPrint;

	public float GetDistanceToActivateNormal()
	{
		return distanceToActivateNormal;
	}

	public float GetDistanceToActivatePrime()
	{
		return distanceToActivatePrime;
	}

	public float GetDistanceToMissStep()
	{
		return distanceToMissStep;
	}

	public float GetDistanceToPlayer(Vector3 myPosition)
	{
		return Vector3.Distance(myPosition, player.transform.position);
	}

	void Start()
	{
		player = FindObjectOfType<Player>();
	}

	public void PlayerSelectedFootprint(Footprint f)
	{
		player.ReportFootprintSelected(f.GetMyFootprintStateObject().GetType());

		if (existingFootprints[0].footprint.GetInstanceID() == f.GetInstanceID())
		{
			existingFootprints.RemoveAt(0);
			Destroy(f.gameObject);

			ActivateNextUp();
		}
		else
		{
			Debug.LogError("Footprint clicked not at front of stack");
		}
	}

	public void FootprintMissed(Footprint f)
	{
		player.ReportFootprintMissed();

		if (f && existingFootprints[0].footprint.GetInstanceID() != f.GetInstanceID())
		{
			Debug.LogError("Footprint clicked not at front of stack");
		}

		for (int i = 0; i < existingFootprints.Count; i++)
		{
			FootprintRegister thisFootprint = existingFootprints[i];
			if (thisFootprint.zPosition - player.transform.position.z < distanceToRemoveFootprintsAfterMissStep)
			{
				existingFootprints.RemoveAt(0);
				i--;
				Destroy(thisFootprint.footprint.gameObject);
			}
			else
			{
				break;
			}
		}

		ActivateNextUp();
	}

	void ActivateNextUp()
	{
		if (existingFootprints.Count > 0
			&& !existingFootprints[0].footprint.GetCanBeSelected())
		{
			existingFootprints[0].footprint.SetAsUpNext();
		}
	}

	public void RegisterFootprint(Footprint f)
	{
		existingFootprints.Add(new FootprintRegister(f, f.transform.position.z));
		existingFootprints.Sort();

		ActivateNextUp();
	}
}

public class FootprintRegister : IComparable
{
	public Footprint footprint;
	public float zPosition;
	public FootprintRegister(Footprint f, float z)
	{
		footprint = f;
		zPosition = z;
	}

	public int CompareTo(object obj)
	{
		FootprintRegister other = (FootprintRegister)obj;
		if (zPosition < other.zPosition)
		{
			return -1;
		}
		return 1;
	}
}