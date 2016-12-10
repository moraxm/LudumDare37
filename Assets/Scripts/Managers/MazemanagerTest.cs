using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazemanagerTest : MonoBehaviour {

  public int widht = 2;
  public int heidht = 2;
  public int seed = 10;

	// Use this for initialization
	void Start () {
    MazeManager m = new MazeManager(widht, heidht, seed);
    m.Build();
	}
	
}
