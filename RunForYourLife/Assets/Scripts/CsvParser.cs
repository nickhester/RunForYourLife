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
				int thisTrackType = -1;
				int thisItemType = -1;
				int thisEnemyType = -1;

				string lineWithoutEnding = _lines[i].Split('\r')[0];
				string[] tokens = lineWithoutEnding.Split(',');
				int a;

				if (int.TryParse((tokens[0]), out a))
				{
					thisTrackType = a;
				}

				if (int.TryParse((tokens[1]), out a))
				{
					thisItemType = a;
				}
				else
				{
					thisItemType = -1;    // default to -1 to mean no spawn, that way the csv file can just be left blank
				}

				if (int.TryParse((tokens[2]), out a))
				{
					thisEnemyType = a;
				}
				else
				{
					thisEnemyType = -1;   // default to -1 to mean no spawn, that way the csv file can just be left blank
				}

				LevelChunk chunk = new LevelChunk(thisTrackType, thisItemType, thisEnemyType);
				_returnLevel.AddTrack(chunk);
			}
		}
		return _returnLevel;

	}

}
