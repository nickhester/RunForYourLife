using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabDrop : MonoBehaviour
{
	[SerializeField] private GameObject prefab;
	[SerializeField] private float chanceToDrop = 1.0f;

	[SerializeField] private bool chooseRandomPrefab = false;
	[SerializeField] private List<GameObject> prefabs;
	
	void Start ()
	{
		if (chanceToDrop >= Random.Range(0.0f, 1.0f))
		{
			GameObject prefabToUse = prefab;
			if (chooseRandomPrefab)
			{
				prefabToUse = prefabs[Random.Range(0, prefabs.Count)];
			}
			GameObject go = Instantiate(prefabToUse) as GameObject;
			go.transform.position = transform.position;
		}
		Destroy(gameObject);
	}
	
	void Update ()
	{
		
	}
}
