using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
	[SerializeField] private Pursuer pursuerPrefab;
	[SerializeField] private Vector2 distanceRangeToNoticePlayer;
	private float distanceToNoticePlayer;
	[SerializeField] private float timeToPursuePlayer;
	private bool hasBegunEntry = false;

	void Start()
	{
		distanceToNoticePlayer = Random.Range(distanceRangeToNoticePlayer.x, distanceRangeToNoticePlayer.y);
	}

	void Update ()
	{
		if (!hasBegunEntry)
		{
			if (Vector3.Distance(transform.position, Player.Instance.transform.position) < distanceToNoticePlayer)
			{
				BeginEntry();
			}
		}
		else
		{
			transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 4.0f), transform.position.z);
		}
	}

	void BeginEntry()
	{
		hasBegunEntry = true;
		Invoke("StartPursuit", timeToPursuePlayer);
	}

	void StartPursuit()
	{
		if (Player.Instance.transform.position.z - transform.position.z > 0.0f)
		{
			GameObject go = Instantiate(pursuerPrefab.gameObject) as GameObject;
			go.transform.position = transform.position;
			Destroy(gameObject);
		}
		else
		{
			print("started pursuit in front of player... you dead");
		}
	}
}
