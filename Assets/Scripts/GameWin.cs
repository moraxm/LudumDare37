using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour {

	public float timeToNextLevel = 4.0f;

	public void WinGame() {
		StartCoroutine(NextLevel());

	}

	private IEnumerator NextLevel() {
		yield return new WaitForSeconds(timeToNextLevel);
		LevelManager.instance.LevelCompleted();
		GetComponent<SceneLoader>().LoadScene("Game");		
	}
}
