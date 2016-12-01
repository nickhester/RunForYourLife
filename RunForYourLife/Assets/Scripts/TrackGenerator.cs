using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
	public List<GameObject> TrackChunkPrefabs;
	private List<GameObject> ChunksCreated;
	private Vector3 nextChunkPosition;

	private Player player;
	public float distanceFromPlayerToSpawnChunk;
	
	void Start ()
	{
		player = FindObjectOfType<Player>();
		nextChunkPosition = new Vector3(0.0f, 0.0f, 5.0f);
	}
	
	void Update ()
	{
		if (Vector3.Distance(nextChunkPosition, player.transform.position) < distanceFromPlayerToSpawnChunk)
		{
			GenerateNextChunk();
		}
	}

	public void GenerateNextChunk()
	{
		GameObject newChunk = Instantiate(TrackChunkPrefabs[Random.Range(0, TrackChunkPrefabs.Count)]) as GameObject;
		newChunk.transform.position = nextChunkPosition;
		UpdateNextChunkPosition();
		ChunksCreated.Add(newChunk);
	}

	void UpdateNextChunkPosition()
	{
		nextChunkPosition.z += 10.0f;
	}
}
