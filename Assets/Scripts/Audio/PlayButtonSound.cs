using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour {

	public void Play() {
		string clip = "click";
		clip += (Random.Range(1, 3)).ToString();
		UtilSound.instance.PlaySound(clip);
	}
}
