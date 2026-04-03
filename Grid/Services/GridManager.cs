using System;
using System.Collections.Generic;
using System.Text;
using Grid.Models;

namespace Grid.Services;

public class GridManager
{
    private readonly Node[,] _nodes;
    public int Width { get; }
    public int Height { get; }

    public GridManager(int width, int height)
    {
        Width = width;
        Height = height;
        _nodes = new Node[width, height];

        // Init every coordinate with a Node object
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _nodes[x, y] = new Node(x, y);
            }
        }
    }

    public Node GetNode(int x, int y) => _nodes[x, y];

    public List<Node> GetNeighbors(Node node, bool allowDiagonals = true)
    {
        var neighbors = new List<Node>();

        for(int dx = -1; dx <=1; dx++)
        {
            for(int dy = -1; dy <= 1; dy++)
            {
                if(dx == 0 && dy == 0)
                {
                    continue; // Skip current node
                }

                if(!allowDiagonals && Math.Abs(dx) + Math.Abs(dy) > 1)
                {
                    continue;
                }

                int newX = node.X + dx;
                int newY = node.Y + dy;

                if (newX >=0 && newX < Width && newY >= 0 && newY < Height)
                {
                    neighbors.Add(_nodes[newX, newY]);
                }
            }
        }
        return neighbors;
    }

    public string RenderGrid()
    {
        var builder = new StringBuilder();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var node = GetNode(x, y);
                char symbol = node.State switch
                {
                    CellState.Blocked => 'O',
                    CellState.Restricted => 'R',
                    _ => '.'
                };

                builder.Append(symbol);
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    public void PrintGrid()
    {
        Console.Write(RenderGrid());
    }

    public override string ToString() => RenderGrid();

}