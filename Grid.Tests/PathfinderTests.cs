using Grid.Models;
using Grid.Services;
using Pathfinder.Services;

namespace Grid.Tests;

public class PathfinderTests
{
    [Fact]
    public void FindPath_OpenGrid_FindsShortestDiagonalPath()
    {
        // Arrange
        var grid = new GridManager(3, 3);
        var pathfinder = new Pathfinder.Services.Pathfinder(grid);
        var start = grid.GetNode(0, 0);
        var target = grid.GetNode(2, 2);

        // Act
        var path = pathfinder.FindPath(start, target);

        // Assert
        Assert.Equal(2, path.Count);
        Assert.Equal((1, 1), (path[0].X, path[0].Y));
        Assert.Equal((2, 2), (path[1].X, path[1].Y));
    }

    [Fact]
    public void FindPath_BlockedMiddleNode_AvoidsBlockedNode()
    {
        // Arrange
        var grid = new GridManager(3, 3);
        grid.GetNode(1, 1).State = CellState.Blocked;

        var pathfinder = new Pathfinder.Services.Pathfinder(grid);
        var start = grid.GetNode(0, 0);
        var target = grid.GetNode(2, 2);

        // Act
        var path = pathfinder.FindPath(start, target);

        // Assert
        Assert.NotEmpty(path);
        Assert.DoesNotContain(path, node => node.State == CellState.Blocked);
        Assert.Equal((2, 2), (path[^1].X, path[^1].Y));
    }
}
