using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour {

	public void WinGame() {
		LevelManager.instance.LevelCompleted();
		GetComponent<SceneLoader>().LoadScene("Game");
	}
}
