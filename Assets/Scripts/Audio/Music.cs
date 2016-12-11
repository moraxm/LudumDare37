using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

	void Start () {
		UtilSound.instance.StopSound("Music");
		UtilSound.instance.PlaySound("Music");
	}
}
