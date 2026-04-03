using System;
using System.Collections.Generic;
using System.Text;

namespace Grid.Models;

public enum CellState
{
    Traversable,
    Blocked,
    Restricted
}

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public CellState State { get; set; } = CellState.Traversable;

    // A* Pathfinding scores
    public float G { get; set; } // Cost from start
    public float H { get; set; } // Heuristic est. distance to end
    public float F => G + H; // Total estimated cost

    public Node Parent { get; set; } // Used to rescuntruct the final path

    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }
}