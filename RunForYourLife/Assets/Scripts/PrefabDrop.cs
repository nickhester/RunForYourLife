using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabDrop : MonoBehaviour
{
	public TrackGenerator.SpawnerType thingsToSpawn;
	
	public void DropPrefab(GameObject prefabToSpawn)
	{
		GameObject go = Instantiate(prefabToSpawn) as GameObject;
		go.transform.SetParent(transform.parent);
		go.transform.position = transform.position;

		Destroy(gameObject);
	}
}
