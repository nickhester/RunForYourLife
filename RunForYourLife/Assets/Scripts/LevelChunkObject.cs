using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunkObject : MonoBehaviour
{
	private LevelChunk m_levelChunk;

	public void Initialize(LevelChunk levelChunk)
	{
		m_levelChunk = levelChunk;

		// trigger spawns
		List<PrefabDrop> dropPoints = new List<PrefabDrop>();
		dropPoints.AddRange(transform.GetComponentsInChildren<PrefabDrop>());
		for (int i = 0; i < dropPoints.Count; i++)
		{
			if (dropPoints[i].thingsToSpawn == TrackGenerator.SpawnerType.ITEM)
			{
				if (m_levelChunk.itemType >= 0)
				{
					dropPoints[i].DropPrefab(TrackGenerator.Instance.items[m_levelChunk.itemType].gameObject);
				}
			}
			else if (dropPoints[i].thingsToSpawn == TrackGenerator.SpawnerType.ENEMY)
			{
				if (m_levelChunk.enemyType >= 0)
				{
					dropPoints[i].DropPrefab(TrackGenerator.Instance.enemies[m_levelChunk.enemyType].gameObject);
				}
			}
			else
			{
				if (dropPoints[i].transform.position.x < 0.0f)
				{
					dropPoints[i].DropPrefab(TrackGenerator.Instance.footprintL.gameObject);
				}
				else
				{
					dropPoints[i].DropPrefab(TrackGenerator.Instance.footprintR.gameObject);
				}
			}
		}
	}
}
