using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;
	}
}
