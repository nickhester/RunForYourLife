using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
	int numTrackTypes;
	int numItemTypes;
	int numEnemyTypes;

	float itemSpawnRatio;
	float enemySpawnRatio;

	public LevelGenerator(int numTrackTypes, int numItemTypes, int numEnemyTypes, float itemSpawnRatio, float enemySpawnRatio)
	{
		this.numTrackTypes = numTrackTypes;
		this.numItemTypes = numItemTypes;
		this.numEnemyTypes = numEnemyTypes;
		this.itemSpawnRatio = itemSpawnRatio;
		this.enemySpawnRatio = enemySpawnRatio;
	}

	public Level Generate(int numChunks)
	{
		Level newLevel = new Level();
		for (int i = 0; i < numChunks; i++)
		{
			bool doesSpawnItem = Random.Range(0.0f, 1.0f) < itemSpawnRatio;
			bool doesSpawnEnemy = Random.Range(0.0f, 1.0f) < enemySpawnRatio;
			LevelChunk newChunk = new LevelChunk(
											Random.Range(0, numTrackTypes), 
											(doesSpawnItem ? Random.Range(0, numItemTypes) : -1), 
											(doesSpawnEnemy ? Random.Range(0, numEnemyTypes) : -1));

			newLevel.AddTrack(newChunk);
		}
		return newLevel;
	}
}
