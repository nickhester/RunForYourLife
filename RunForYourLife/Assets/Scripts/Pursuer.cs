using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pursuer : MonoBehaviour
{
	[SerializeField] private float startingMoveSpeed;
	private float currentMoveSpeed;
	private Player player;
	private Slider sliderDistanceFromPlayer;
	[SerializeField] private float distanceFromPlayerToStartSliderProgress;

	void Start ()
	{
		currentMoveSpeed = startingMoveSpeed;

		player = FindObjectOfType<Player>();
		sliderDistanceFromPlayer = GetComponentInChildren<Slider>();
	}
	
	void Update ()
	{
		if (!player.GetPlayerHasBeenCaught())
		{
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

			float distanceFromPlayer = player.transform.position.z - transform.position.z;
			float slidervalue = Mathf.Clamp01(-(distanceFromPlayer / distanceFromPlayerToStartSliderProgress) + 1.0f);
			sliderDistanceFromPlayer.value = slidervalue;
			//print("DISTANCE: " + (player.transform.position.z - transform.position.z) + "Slider: " + slidervalue);

			if (distanceFromPlayer <= 0.0f)
			{
				sliderDistanceFromPlayer.gameObject.SetActive(false);
				player.ReportPursuerCaughtPlayer();
			}
		}
	}
}
