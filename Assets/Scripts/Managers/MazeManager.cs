using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager
{

  private int Widht;
  private int Height;

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

  public Cell[,] Build(int widht, int height)
  {
    m_maze = new Cell[widht, height];

    for(int i = 0; i < widht; ++i)
    {
      for(int j = 0; j < height; ++j)
      {
        Cell c = new Cell();
        c.Top = Random.value < 0.5f;
        c.Bottom = Random.value < 0.5f;
        c.Left = Random.value < 0.5f;
        c.Right = Random.value < 0.5f;
        m_maze[i, j] = c;
      }
    }
    return m_maze;
  }
}
