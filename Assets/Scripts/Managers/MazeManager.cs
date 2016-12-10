using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager
{
  private int m_widht;
  private int m_height;
  private System.Random m_random;

  #region variables to return
  public struct Cell
  {
    public bool Top;
    public bool Bottom;
    public bool Left;
    public bool Right;
  }
  private Cell[,] m_maze;
  public Cell[,] Maze
  {
    get { return m_maze; }
  }
  #endregion
  #region private structs
  private struct Wall
  {
    public Wall(int x1, int y1, int x2, int y2)
    {
      this.x1 = x1;
      this.x2 = x2;
      this.y1 = y1;
      this.y2 = y2;
    }
    public int x1;
    public int y1;
    public int x2;
    public int y2;
  }
  private struct BuildCell
  {
    public int x;
    public int y;

    public BuildCell(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
  }
  #endregion

  #region variables to build
  List<HashSet<BuildCell>> hashCells;
  List<Wall> wallList;
  List<Wall> finalWalls;
  #endregion

  #region constructor
  public MazeManager(int widht, int height, int seed)
  {
    m_widht = widht;
    m_height = height;
    m_random = new System.Random(seed);
  }
  
  public MazeManager(int widht, int height)
  {
    m_widht = widht;
    m_height = height;
    int seed = (int)(DateTime.Now.Ticks);
    m_random = new System.Random(seed);
  }
  
  #endregion

  public Cell[,] Build()
  {
    m_maze = new Cell[m_widht, m_height];
    GetBuildCells();
    GetWallList();
    finalWalls = new List<Wall>();
    BuildMaze();
    BuildStructure();
    PrintMaze();
    return m_maze;
  }

  private void GetBuildCells()
  {
    hashCells = new List<HashSet<BuildCell>>();
    for (int i = 0; i < m_widht; ++i)
    {
      for (int j = 0; j < m_height; ++j)
      {
        BuildCell b = new BuildCell();
        b.x = i;
        b.y = j;
        HashSet<BuildCell> hash = new HashSet<BuildCell>();
        hash.Add(b);
        hashCells.Add(hash);
      }
    }
  }

  private void GetWallList()
  {
    wallList = new List<Wall>();
    for (int i = 0; i < m_widht; ++i)
    {
      for (int j = 0; j < m_height; ++j)
      {
        //top
        Wall top = new Wall(i, j, i, j + 1);
        Wall topInverse = new Wall(i, j + 1, i, j);

        if (!wallList.Contains(top) && !wallList.Contains(topInverse) && j + 1 < m_height)
        {
          wallList.Add(top);
        }
        //bottom
        Wall bottom = new Wall(i, j, i, j - 1);
        Wall bottomInverse = new Wall(i, j - 1, i, j);
        if (!wallList.Contains(bottom) && !wallList.Contains(bottomInverse) && j - 1 > 0)
        {
          wallList.Add(bottom);
        }
        //left
        Wall left = new Wall(i, j, i - 1, j);
        Wall leftInverse = new Wall(i - 1, j, i, j);
        if (!wallList.Contains(left) && !wallList.Contains(leftInverse) && i - 1 > 0)
        {
          wallList.Add(left);
        }
        //right
        Wall right = new Wall(i, j, i + 1, j);
        Wall rightInverse = new Wall(i + 1, j, i, j);
        if (!wallList.Contains(right) && !wallList.Contains(rightInverse) && i + 1 < m_widht)
        {
          wallList.Add(right);
        }
      }
    }
  }
  private void BuildMaze()
  {
    while (wallList.Count != 0)
    {
      int randomIndex = m_random.Next(0, wallList.Count);
      Wall wall = wallList[randomIndex];
      wallList.RemoveAt(randomIndex);

      BuildCell b1 = new BuildCell(wall.x1, wall.y1);
      BuildCell b2 = new BuildCell(wall.x2, wall.y2);
      int indexForb1 = HashWithCell(b1);
      int indexForb2 = HashWithCell(b2);
      if (indexForb1 != indexForb2)
      {
        MergeHash(indexForb1, indexForb2);
      }
      else
      {
        finalWalls.Add(wall);
      }
    }

  }
  private int HashWithCell(BuildCell b)
  {
    for (int i = 0; i < hashCells.Count; ++i)
    {
      if (hashCells[i].Contains(b))
      {
        return i;
      }
    }
    return -1;
  }
  private void MergeHash(int index1, int index2)
  {
    HashSet<BuildCell> has1 = hashCells[index1];
    HashSet<BuildCell> has2 = hashCells[index2];
    has1.UnionWith(has2);
    //hashCells.Add(has1);
    hashCells.RemoveAt(index2);
  }
  private void BuildStructure()
  {
    BuildStructureBorders();
    
    for (int i = 0; i < finalWalls.Count; ++i)
    {
      Wall w = finalWalls[i];
      if (w.x1 == w.x2)
      {
        if (w.y1 > w.y2)
        {
          m_maze[w.x1, w.y1].Bottom = true;
          m_maze[w.x2, w.y2].Top = true;
        }
        else
        {
          m_maze[w.x1, w.y1].Top = true;
          m_maze[w.x2, w.y2].Bottom = true;
        }
      }
      else
      {
        if (w.x1 < w.x2)
        {
          m_maze[w.x1, w.y1].Right = true;
          m_maze[w.x2, w.y2].Left = true;
        }
        else
        {
          m_maze[w.x1, w.y1].Left = true;
          m_maze[w.x2, w.y2].Right = true;
        }
      }
    }
    
  }

  private void BuildStructureBorders()
  {
    //botttom
    for (int i = 0; i < m_widht; ++i)
    {
      m_maze[i, 0].Bottom = true;
    }

    //Left
    for (int i = 0; i < m_height; ++i)
    {
      m_maze[0, i].Left = true;
    }

    //Top
    for (int i = 0; i < m_widht; ++i)
    {
      m_maze[i, m_height - 1].Top = true;
    }

    //Right
    for (int i = 0; i < m_height; ++i)
    {
      m_maze[m_widht - 1, i].Right = true;
    }
  }

  private void PrintMaze()
  {
    for (int j = m_height - 1; j >= 0; --j)
    {
      string line = "";
      for (int i = 0; i < m_widht; ++i)
      {
        Cell c = m_maze[i, j];
        if(c.Left)
        {
          line += "|";
        }
        line += "*";
        if(c.Top && c.Bottom)
        {
          line += "=";
        }
        
        if(c.Top && !c.Bottom)
        {
          line += "-";
        }
        if(!c.Top && c.Bottom)
        {
          line += "_";
        }

        if(c.Right)
        {
          line += "|";
        }
      }
      Debug.Log(line);
    }
  }
}
