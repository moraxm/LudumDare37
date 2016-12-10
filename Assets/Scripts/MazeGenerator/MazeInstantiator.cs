using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

enum ORIENTATION { LEFT, RIGHT, TOP, BOTTOM };

public class MazeInstantiator : MonoBehaviour
{

    private MazeManager mazeManager = null;

    public int width = 5;
    public int height = 5;
    public Vector2 tileSize = new Vector2(5.0f, 5.0f);
    public Vector3 origin = Vector3.zero;
    public float mazeHeight = 2.0f;
    public GameObject wallPrefab = null;
    public GameObject cornerPrefab = null;
    public GameObject floorPrefab = null;
    public GameObject endLinePrefab = null;
    private GameObject tilePrefab = null;

    // Use this for initialization
    void Start()
    {
        if (!wallPrefab || !cornerPrefab || !floorPrefab || width <= 0 || height <= 0 || tileSize.x <= 0.0f || tileSize.y <= 0.0f || mazeHeight < 0.0f)
        {
            Debug.LogError("[MazeInstantiator.cs] Error. Missing a public property in MazeInstantiator");
        }
        StartCoroutine(InstantiateMaze());
    }

    private IEnumerator InstantiateMaze()
    {
        mazeManager = new MazeManager(width, height);
        mazeManager.Build();

        // Place Labyrinth global position
        gameObject.transform.position = new Vector3(origin.x, origin.y, origin.z);

        // Instantiate Floor
        GameObject floor = Instantiate(floorPrefab, this.gameObject.transform) as GameObject;
        Vector3 pos = new Vector3((width - 1) * tileSize.x / 2.0f, -mazeHeight / 2.0f, (height - 1) * tileSize.y / 2.0f);
        floor.transform.localPosition = pos;
        Vector3 scale = new Vector3(width * tileSize.x, 0.1f, height * tileSize.y);
        floor.transform.localScale = scale;
        floor.name = "Floor";
        // Floor texture tiling
        floor.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(scale.x, scale.z));

        // Place camera
        Camera.main.transform.position = new Vector3(pos.x, Camera.main.transform.position.y, pos.z);

        int endLineX = width - 1;
        int endLineY = height - 1;

        // Instantiate Tiles
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                yield return StartCoroutine(InstantiateTile(i, j));
                if (i == endLineX && j == endLineY)
                {
                    InstantiateEndLine();
                }
            }
        }
    }

    private IEnumerator InstantiateTile(int i, int j)
    {
        GameObject tile = tilePrefab ? Instantiate(tilePrefab) as GameObject : new GameObject();
        if (tile)
        {
            tile.transform.parent = this.gameObject.transform;
            tile.transform.localPosition = new Vector3(i * tileSize.x, 0.0f, j * tileSize.y); // Each row is placed in a deeper -Z
            tile.transform.localScale = new Vector3(tileSize.x, 1.0f, tileSize.y);
            tile.name = "Tile" + i.ToString() + "_" + j.ToString();
            if (mazeManager.Maze[i, j].Left)
            {
                yield return StartCoroutine(CreateWall(i, j, ORIENTATION.LEFT, tile));
            }
            if (mazeManager.Maze[i, j].Right)
            {
                yield return StartCoroutine(CreateWall(i, j, ORIENTATION.RIGHT, tile));
            }
            if (mazeManager.Maze[i, j].Top)
            {
                yield return StartCoroutine(CreateWall(i, j, ORIENTATION.TOP, tile));
            }
            if (mazeManager.Maze[i, j].Bottom)
            {
                yield return StartCoroutine(CreateWall(i, j, ORIENTATION.BOTTOM, tile));
            }

            yield return StartCoroutine(CreateCorners(i, j, tile));
        }
    }

    private IEnumerator CreateCorners(int i, int j, GameObject tile)
    {
        GameObject corner = Instantiate(cornerPrefab, tile.transform) as GameObject;
        corner.transform.localScale = new Vector3(1.0f / tileSize.x, mazeHeight, 1.0f / tileSize.y);
        corner.transform.localPosition = new Vector3(((-tileSize.x / 2.0f) + (corner.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x, 0.0f, ((tileSize.y / 2.0f) - (corner.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z);
        corner.name = "CornerUpperLeft";

        corner = Instantiate(cornerPrefab, tile.transform) as GameObject;
        corner.transform.localScale = new Vector3(1.0f / tileSize.x, mazeHeight, 1.0f / tileSize.y);
        corner.transform.localPosition = new Vector3(((tileSize.x / 2.0f) - (corner.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x, 0.0f, ((tileSize.y / 2.0f) - (corner.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z);
        corner.name = "CornerUpperRight";

        corner = Instantiate(cornerPrefab, tile.transform) as GameObject;
        corner.transform.localScale = new Vector3(1.0f / tileSize.x, mazeHeight, 1.0f / tileSize.y);
        corner.transform.localPosition = new Vector3(((-tileSize.x / 2.0f) + (corner.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x, 0.0f, ((-tileSize.y / 2.0f) + (corner.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z);
        corner.name = "CornerLowerLeft";

        corner = Instantiate(cornerPrefab, tile.transform) as GameObject;
        corner.transform.localScale = new Vector3(1.0f / tileSize.x, mazeHeight, 1.0f / tileSize.y);
        corner.transform.localPosition = new Vector3(((tileSize.x / 2.0f) - (corner.GetComponent<MeshRenderer>().bounds.size.x / 2.0f)) / tile.transform.localScale.x, 0.0f, ((-tileSize.y / 2.0f) + (corner.GetComponent<MeshRenderer>().bounds.size.z / 2.0f)) / tile.transform.localScale.z);
        corner.name = "CornerLowerRight";

        yield return null;
    }

    private IEnumerator CreateWall(int i, int j, ORIENTATION orientation, GameObject tile)
    {
        GameObject wall = Instantiate(wallPrefab, tile.transform) as GameObject;

        if (orientation == ORIENTATION.TOP || orientation == ORIENTATION.BOTTOM)
        {
            wall.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f), Space.Self);
        }

        // Calculate Scale
        Vector3 scale = new Vector3(1.0f / tileSize.x, mazeHeight, 0.60f);
        wall.transform.localScale = scale;

        // Calculate Position
        Vector3 pos = Vector3.zero;
        switch (orientation)
        {
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
        wall.name = "Wall" + orientation.ToString();

        yield return null;
    }

    private void InstantiateEndLine()
    {
        GameObject endLine = Instantiate(endLinePrefab) as GameObject;
        Vector3 scale = Vector3.one;
        scale.x = tileSize.x;
        scale.z = tileSize.y;
        endLine.transform.localScale = scale;
        endLine.name = "End Line";
        endLine.transform.localPosition = new Vector3(origin.x + (width - 1) * tileSize.x, 0, origin.y + (height - 1) * tileSize.y);
    }

}
