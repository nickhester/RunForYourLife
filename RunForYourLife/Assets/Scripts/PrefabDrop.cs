using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabDrop : MonoBehaviour
{
	enum SpawnMethod
	{
		CENTRAL,
		SPECIFIC,
		CUSTOM_LIST
	}
	[SerializeField] private SpawnMethod mySpawnMethod;
	[SerializeField] private TrackGenerator.ThingsToSpawn thingsToSpawn;

	[SerializeField] private GameObject prefab;
	[SerializeField] private float chanceToDrop = 1.0f;
	
	[SerializeField] private List<GameObject> prefabs;

	
	
	void Start ()
	{
		if (chanceToDrop >= Random.Range(0.0f, 1.0f))
		{
			GameObject prefabToUse = null;

			switch (mySpawnMethod)
			{
				case SpawnMethod.CENTRAL:
				{
					TrackGenerator trackGenerator = FindObjectOfType<TrackGenerator>();

					switch (thingsToSpawn)
					{
						case TrackGenerator.ThingsToSpawn.ENEMY:
						{
							prefabToUse = trackGenerator.GetEnemyPrefabToSpawn().gameObject;
							break;
						}
						case TrackGenerator.ThingsToSpawn.ITEM:
						{
							prefabToUse = trackGenerator.GetItemPrefabToSpawn().gameObject;
							break;
						}
						default:
						{
							break;
						}
					}
					break;
				}
				case SpawnMethod.SPECIFIC:
				{
					prefabToUse = prefab;
					break;
				}
				case SpawnMethod.CUSTOM_LIST:
				{
					prefabToUse = prefabs[Random.Range(0, prefabs.Count)];
					break;
				}
				default:
				{
					break;
				}
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
