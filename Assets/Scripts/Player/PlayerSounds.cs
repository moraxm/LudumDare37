using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

	void OnCollisionEnter(Collision other) {
		if (other.collider.gameObject.tag != "Floor") {
			string clip = "choque";
			clip += (Random.Range(1, 4)).ToString();
			UtilSound.instance.PlaySound(clip);
		}
	}
}
