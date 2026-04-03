using Grid.Models;
using Grid.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinder.Services;

public class Pathfinder
{
    private readonly GridManager _grid;

    public Pathfinder(GridManager grid) => _grid = grid;

    public List<Node> FindPath(Node start, Node target)
    {
        // Sorting (F-Score)
        var openSet = new PriorityQueue<Node, float>();
        // Is it already in the queue
        var openSetNodes = new HashSet<Node>();
        var closedSet = new HashSet<Node>();

        start.G = 0;
        start.H = GetDistance(start, target);

        openSet.Enqueue(start, start.F);
        openSetNodes.Add(start);

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();
            openSetNodes.Remove(current);

            if (current == target) return RetracePath(start, target);

            closedSet.Add(current);

            foreach (var neighbor in _grid.GetNeighbors(current))
            {
                // Skip if blocked or already fully evaluated
                if (neighbor.State == CellState.Blocked || closedSet.Contains(neighbor))
                    continue;

                float newMovementCost = current.G + GetDistance(current, neighbor);

                // If this is a shorter path or the neighbor hasn't been visited
                if (newMovementCost < neighbor.G || !openSetNodes.Contains(neighbor))
                {
                    neighbor.G = newMovementCost;
                    neighbor.H = GetDistance(neighbor, target);
                    neighbor.Parent = current;

                    if (!openSetNodes.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, neighbor.F);
                        openSetNodes.Add(neighbor);
                    }
                }
            }
        }
        return new List<Node>(); // Return empty if no path exists
    }

    private float GetDistance(Node a, Node b)
    {
        int dx = Math.Abs(a.X - b.X);
        int dy = Math.Abs(a.Y - b.Y);
        // Octile distance for 8-way movement
        return dx > dy ? 1.41f * dy + (dx - dy) : 1.41f * dx + (dy - dx);
    }

    private List<Node> RetracePath(Node start, Node end)
    {
        var path = new List<Node>();
        var current = end;
        while (current != start) 
        { 
            path.Add(current); 
            current = current.Parent; 
        }
        path.Reverse();
        return path;
    }
}
