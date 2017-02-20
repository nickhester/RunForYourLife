using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
	public List<GameObject> trackChunkPrefabs;
	public GameObject goalChunk;
	private GameObject goalChunkInstance;
	private List<GameObject> chunksCreated = new List<GameObject>();
	private Vector3 nextChunkPosition;
	
	public float distanceFromPlayerToSpawnChunk;

	[SerializeField] private float checkChunksToDestroyInterval;
	private float checkChunksToDestroyCounter = 0.0f;
	[SerializeField] private float distanceBehindPlayerToDestroyChunks;

	private Level currentLevel;
	private int currentChunkNumber = 0;

	public enum SpawnerType
	{
		ENEMY,
		ITEM,
		FOOTPRINT
	}
	public List<Zombie> enemies = new List<Zombie>();
	public List<Item> items = new List<Item>();
	public Footprint footprintL;
	public Footprint footprintR;

	// singleton instance
	public static TrackGenerator Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<TrackGenerator>();

			return _instance;
		}
	}
	private static TrackGenerator _instance;


void Start ()
	{
		nextChunkPosition = new Vector3(0.0f, 0.0f, 5.0f);

		GameManager gm = FindObjectOfType<GameManager>();
		if (gm != null && gm.LevelToLoad != "")
		{
			currentLevel = CsvParser.DeseriealizeLevel(gm.LevelToLoad);
		}
		else
		{
			Debug.LogWarning("Level from GM not found, loading test level");
			currentLevel = CsvParser.DeseriealizeLevel("LevelTest");
		}
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
		if (currentChunkNumber < currentLevel.tracks.Count)     // keep generating chunks until you've exceeded number of chunks
		{
			LevelChunk currentLevelChunk = currentLevel.tracks[currentChunkNumber];

			if (currentLevelChunk.trackType >= trackChunkPrefabs.Count)
				Debug.LogError("Level requested chunk type outside of range");

			GameObject newChunkGameObject = Instantiate(
													trackChunkPrefabs[currentLevelChunk.trackType],
													nextChunkPosition,
													Quaternion.identity,
													transform) as GameObject;
			LevelChunkObject newChunkObject = newChunkGameObject.GetComponent<LevelChunkObject>();
			newChunkObject.Initialize(currentLevelChunk);
			currentChunkNumber++;

			UpdateNextChunkPosition();
			chunksCreated.Add(newChunkGameObject);
		}
		else
		{
			if (goalChunkInstance == null)
			{
				goalChunkInstance = Instantiate(
					goalChunk,
					nextChunkPosition,
					Quaternion.identity,
					transform) as GameObject;
			}
		}
	}

	void UpdateNextChunkPosition()
	{
		nextChunkPosition.z += 10.0f;
	}
	
}
