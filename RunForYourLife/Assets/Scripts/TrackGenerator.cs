using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
	public List<GameObject> trackChunkPrefabs;
	private List<GameObject> chunksCreated = new List<GameObject>();
	private Vector3 nextChunkPosition;
	
	public float distanceFromPlayerToSpawnChunk;

	[SerializeField] private float checkChunksToDestroyInterval;
	private float checkChunksToDestroyCounter = 0.0f;
	[SerializeField] private float distanceBehindPlayerToDestroyChunks;

	public enum ThingsToSpawn
	{
		ENEMY,
		ITEM
	}
	public List<Zombie> enemies = new List<Zombie>();
	public List<Item> items = new List<Item>();
	
	void Start ()
	{
		nextChunkPosition = new Vector3(0.0f, 0.0f, 5.0f);
	}
	
	void Update ()
	{
		checkChunksToDestroyCounter += Time.deltaTime;
		if (checkChunksToDestroyCounter > checkChunksToDestroyInterval)
		{
			DestroyChunksBehindPlayer();

			checkChunksToDestroyCounter = 0.0f;
		}

		if (Vector3.Distance(nextChunkPosition, Player.Instance.transform.position) < distanceFromPlayerToSpawnChunk)
		{
			GenerateNextChunk();
		}
	}

	void DestroyChunksBehindPlayer()
	{
		for (int i = 0; i < chunksCreated.Count; i++)
		{
			if ((Player.Instance.transform.position.z - chunksCreated[i].transform.position.z) > distanceBehindPlayerToDestroyChunks)
			{
				Destroy(chunksCreated[i]);
				chunksCreated.RemoveAt(i);
				i--;
			}
		}
	}

	public void GenerateNextChunk()
	{
		GameObject newChunk = Instantiate(trackChunkPrefabs[Random.Range(0, trackChunkPrefabs.Count)]) as GameObject;
		newChunk.transform.position = nextChunkPosition;
		UpdateNextChunkPosition();
		chunksCreated.Add(newChunk);
	}

	void UpdateNextChunkPosition()
	{
		nextChunkPosition.z += 10.0f;
	}

	public Zombie GetEnemyPrefabToSpawn()
	{
		return enemies[Random.Range(0, enemies.Count)];
	}

	public Item GetItemPrefabToSpawn()
	{
		return items[Random.Range(0, items.Count)];
	}
}
