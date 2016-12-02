using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabDrop : MonoBehaviour
{
	[SerializeField] private GameObject prefab;
	[SerializeField] private float chanceToDrop = 1.0f;

	
	void Start ()
	{
		if (chanceToDrop >= Random.Range(0.0f, 1.0f))
		{
			GameObject go = Instantiate(prefab) as GameObject;
			go.transform.position = transform.position;
		}
		Destroy(gameObject);
	}
	
	void Update ()
	{
		
	}
}
