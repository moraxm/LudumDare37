using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	static LevelManager m_instance;
	public static LevelManager instance
	{
		get
		{
			return m_instance;
		}
	}

	private int _currentLevel = 0;
	public int CurrentLevel {
		get { return _currentLevel; }
	}

	public void Awake()
	{
		if (m_instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			m_instance = this;
			DontDestroyOnLoad(m_instance);
		}
	}

	// Use this for initialization
	void Start () {
		_currentLevel = 0;
	}
	
	public void LevelCompleted() {
		++_currentLevel;
	}
}
