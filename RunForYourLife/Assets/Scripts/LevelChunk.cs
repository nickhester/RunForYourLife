using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk
{
	public int trackType;
	public int itemType;
	public int enemyType;

	public LevelChunk(int trackType, int itemType, int enemyType)
	{
		this.trackType = trackType;
		this.itemType = itemType;
		this.enemyType = enemyType;
	}
}
