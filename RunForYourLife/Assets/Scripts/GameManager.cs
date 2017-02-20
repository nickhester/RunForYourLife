using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	private string levelToLoad;
	public string LevelToLoad
	{
		get
		{
			Destroy(this.gameObject, 0.1f);
			return levelToLoad;
		}
	}

	void Start ()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void StartLevel(string s)
	{
		levelToLoad = s;
		SceneManager.LoadScene("Main");
	}
}
