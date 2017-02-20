using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CsvParser {


	public static Level DeseriealizeLevel(string _levelName)
	{
		Level _returnLevel = new Level();

		FileIO fileIO = new FileIO(_levelName, "csv");

		string fileText = fileIO.GetFileText();
		
		List<string> _lines = new List<string>();
		_lines.AddRange(fileText.Split('\n'));
		if (_lines.Contains("")) { _lines.Remove(""); }   // remove possible trailing line

		for (int i = 1; i < _lines.Count; i++)	// start on index 1 to skip the header
		{
			if (_lines[i].StartsWith("//"))
			{
				// it's a comment, ignore it
			}
			else
			{
				LevelChunk chunk = new LevelChunk();
				
				string lineWithoutEnding = _lines[i].Split('\r')[0];
				string[] tokens = lineWithoutEnding.Split(',');
				int a;

				if (int.TryParse((tokens[0]), out a))
				{
					chunk.trackType = a;
				}

				if (int.TryParse((tokens[1]), out a))
				{
					chunk.itemType = a;
				}
				else
				{
					chunk.itemType = -1;    // default to -1 to mean no spawn, that way the csv file can just be left blank
				}

				if (int.TryParse((tokens[2]), out a))
				{
					chunk.enemyType = a;
				}
				else
				{
					chunk.enemyType = -1;   // default to -1 to mean no spawn, that way the csv file can just be left blank
				}

				_returnLevel.tracks.Add(chunk);
			}
		}
		return _returnLevel;

	}

}
