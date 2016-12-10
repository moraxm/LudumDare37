using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameSound : MonoBehaviour {

	public void PlayGameOver() {
		UtilSound.instance.PlaySound("gameover");
	}

	public void PlayVictory() {
		UtilSound.instance.PlaySound("victory");
	}
}
