using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	List<LevelChunk> tracks = new List<LevelChunk>();

	public int TrackLength
	{
		get
		{
			return tracks.Count;
		}
	}

	public void AddTrack(LevelChunk c)
	{
		tracks.Add(c);
	}

	public void AddTracks(List<LevelChunk> c)
	{
		tracks.AddRange(c);
	}

	public LevelChunk GetTrackAt(int index)
	{
		if (index >= tracks.Count)
		{
			Debug.LogError("Accessing track index that's out of range");
		}
		return tracks[index];
	}
}
