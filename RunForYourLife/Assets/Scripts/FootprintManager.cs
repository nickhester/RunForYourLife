using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FootprintManager : MonoBehaviour
{
	private List<FootprintRegister> existingFootprints = new List<FootprintRegister>();

	[SerializeField] private float distanceToActivateNormal;
	[SerializeField] private float distanceToActivatePrime;
	[SerializeField] private float distanceToMissStep;
	[SerializeField] private float distanceToRemoveFootprintsAfterMissStep;
	[SerializeField] private float delayAfterMissStep;
	
	public Texture inactiveFootPrint;
	public Texture activeFootPrint;
	public Texture primeFootPrint;

	[SerializeField] private ParticleSystem partiPrimeStepPrefab;
	[SerializeField] private ParticleSystem partiNormalStepPrefab;
	private List<GameObject> primeParticleEffects = new List<GameObject>();

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
		return Vector3.Distance(myPosition, Player.Instance.transform.position);
	}
	
	void SpawnStepParticleEffect(Vector3 location, ParticleSystem parti)
	{
		GameObject go = Instantiate(parti.gameObject) as GameObject;
		go.transform.position = location;
		primeParticleEffects.Add(go);
		Invoke("DestroyParticleEffect", 4.0f);
	}

	void DestroyParticleEffect()
	{
		if (primeParticleEffects.Count > 0)
		{
			Destroy(primeParticleEffects[0]);
			primeParticleEffects.RemoveAt(0);
		}
	}

	public void PlayerSelectedFootprint(Footprint f)
	{
		Player.Instance.ReportFootprintSelected(f.GetMyFootprintStateObject().GetType());

		if (f.GetMyFootprintStateObject().GetType() == typeof(FootprintStatePrime))
		{
			SpawnStepParticleEffect(f.transform.position, partiPrimeStepPrefab);
		}
		else
		{
			SpawnStepParticleEffect(f.transform.position, partiNormalStepPrefab);
		}

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
		Player.Instance.ReportFootprintMissed();

		if (f && existingFootprints[0].footprint.GetInstanceID() != f.GetInstanceID())
		{
			Debug.LogError("Footprint clicked not at front of stack");
		}

		for (int i = 0; i < existingFootprints.Count; i++)
		{
			FootprintRegister thisFootprint = existingFootprints[i];
			if (thisFootprint.zPosition - Player.Instance.transform.position.z < distanceToRemoveFootprintsAfterMissStep)
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

	public void TriggerFootprintButtonPress(bool isLeftFoot)
	{
		Footprint nextFootstepUp = existingFootprints[0].footprint;
		if (nextFootstepUp.isLeftFoot == isLeftFoot
			&& nextFootstepUp.GetCanBeSelected())
		{
			PlayerSelectedFootprint(nextFootstepUp);
		}
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