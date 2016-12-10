﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ORIENTATION {LEFT, RIGHT, TOP, BOTTOM};

public class MazeInstantiator : MonoBehaviour {

    private Cell[,] maze = null;

    public int width = 5;
    public int height = 5;
    public Vector2 tileSize = new Vector2(5.0f, 5.0f);
    public Vector2 origin = Vector2.zero;
    public float yPositionForWholeMaze = 0.0f;
    public GameObject wallPrefab = null;
    public GameObject floorPrefab = null;
	public GameObject tilePrefab = null;

	// Use this for initialization
	void Start () {
        if (!wallPrefab || !floorPrefab || width <= 0 || height <= 0 || tileSize.x <= 0.0f || tileSize.y <= 0.0f) {
            Debug.LogError("[MazeInstantiator.cs] Error. Missing a public property in MazeInstantiator");
        }
        StartCoroutine(InstantiateMaze());
	}
	
    private IEnumerator InstantiateMaze () {
        maze = MazeManager.Build(width, height);
        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
				yield return StartCoroutine(InstantiateTile(i, j));
            }
        }
	}

    private IEnumerator InstantiateTile (int i, int j) {
		GameObject tile = tilePrefab ? Instantiate(tilePrefab) as GameObject : new GameObject();
        if (tile) {
            tile.transform.parent = this.gameObject.transform;
            tile.transform.localPosition = new Vector3(origin.x + i * tileSize.x, yPositionForWholeMaze, origin.y - j * tileSize.y); // Each row is placed in a deeper -Z
			tile.transform.localScale = new Vector3(tileSize.x, 1.0f, tileSize.y);
            tile.name = "Tile" + i.ToString() + "_" + j.ToString();
            if (maze[i, j].Left) {
				yield return StartCoroutine(CreateWall(i, j, ORIENTATION.LEFT, tile));
			}
            if (maze[i, j].Right) {
				yield return StartCoroutine(CreateWall(i, j, ORIENTATION.RIGHT, tile));
            }
            if (maze[i, j].Top) {
				yield return StartCoroutine(CreateWall(i, j, ORIENTATION.TOP, tile));
            }
            if (maze[i, j].Bottom) {
				yield return StartCoroutine(CreateWall(i, j, ORIENTATION.BOTTOM, tile));
            }
        }
    }

    private IEnumerator CreateWall(int i, int j, ORIENTATION orientation, GameObject tile) {
		GameObject wall = Instantiate(wallPrefab, tile.transform) as GameObject;
		wall.name = "Wall" + orientation.ToString();

		// Calculate Scale
		Vector3 scale = Vector3.one;
		if (orientation == ORIENTATION.LEFT || orientation == ORIENTATION.RIGHT) {
			scale.x = 0.15f;
		} else {
			scale.z = 0.15f;
		}
		wall.transform.localScale = scale; 

		// Calculate Position
		Vector3 pos = Vector3.zero;
		switch (orientation) {
		case ORIENTATION.LEFT:
			pos.x = ((-tileSize.x / 2.0f) + (wall.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x;
			break;
		case ORIENTATION.RIGHT:
			pos.x = ((tileSize.x / 2.0f) - (wall.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x;
			break;
		case ORIENTATION.TOP:
			pos.z = ((tileSize.y / 2.0f) - (wall.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z;
			break;
		case ORIENTATION.BOTTOM:
			pos.z = ((-tileSize.y / 2.0f) + (wall.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z;
			break;
		}
		wall.transform.localPosition = pos;

        yield return null;
    }
}