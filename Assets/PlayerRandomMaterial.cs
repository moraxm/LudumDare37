using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomMaterial : MonoBehaviour {

	public Material[] materials;

	private void Awake() {
		if (materials.Length > 0) {
			int num = Random.Range(0, materials.Length);
			gameObject.GetComponent<Renderer>().material = materials[num];
		}
	}
}
