using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

	void Start () {
		UtilSound.instance.StopSound("music");
		UtilSound.instance.PlaySound("music", 0.075f, true);
	}
}
