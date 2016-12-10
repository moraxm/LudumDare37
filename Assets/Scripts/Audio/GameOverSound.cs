using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSound : MonoBehaviour {

	public void Play() {
		UtilSound.instance.PlaySound("gameover");
	}
}
