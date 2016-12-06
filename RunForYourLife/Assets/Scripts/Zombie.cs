using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
	[SerializeField] private Pursuer pursuerPrefab;
	[SerializeField] private Vector2 distanceRangeToNoticePlayer;
	private float distanceToNoticePlayer;
	[SerializeField] private float timeToPursuePlayer;
	[SerializeField] private float playerPassTimerMultiplier;
	private float counter = 0.0f;
	private bool hasBegunEntry = false;
	private bool hasPassedPlayer = false;

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
				hasBegunEntry = true;
			}
		}
		else
		{
			if (Player.Instance.transform.position.z - transform.position.z > 0.0f)
			{
				hasPassedPlayer = true;
			}

			// wiggle so it's obvious when he's been alerted
			transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 6.0f), transform.position.z);

			float timeMultiplier = (hasPassedPlayer ? playerPassTimerMultiplier : 1.0f);
			counter += Time.deltaTime * timeMultiplier;
			if (counter > timeToPursuePlayer)
			{
				StartPursuit();
			}
		}
	}

	void StartPursuit()
	{
		if (Player.Instance.transform.position.z - transform.position.z > 0.0f)
		{
			print("zombie starting pursuit");
			GameObject go = Instantiate(pursuerPrefab.gameObject) as GameObject;
			go.transform.position = transform.position;
			Destroy(gameObject);
		}
		else
		{
			Player.Instance.ReportPursuerCaughtPlayer();
		}
	}
}
