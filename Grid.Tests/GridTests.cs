using Grid.Services;
using Grid.Models;
using System.IO;

namespace Grid.Tests;

public class GridTests
{
    [Fact]
    public void Grid_Initialization_SetsCorrectDimensions()
    {
        // Arrange & Act
        var manager = new GridManager(10, 20);

        // Assert
        Assert.Equal(10, manager.Width);
        Assert.Equal(20, manager.Height);
    }

    [Fact]
    public void GetNeighbors_CenterNode_ReturnsEightNeighbors()
    {
        // Arrange
        var manager = new GridManager(5, 5);
        var centerNode = manager.GetNode(2, 2);

        // Act
        var neighbors = manager.GetNeighbors(centerNode, allowDiagonals: true);

        // Assert
        Assert.Equal(8, neighbors.Count);
    }

    [Fact]
    public void GetNeighbors_CornerNode_ReturnsThreeNeighbors()
    {
        // Arrange
        var manager = new GridManager(5, 5);
        var cornerNode = manager.GetNode(0, 0); // Top-left corner

        // Act
        var neighbors = manager.GetNeighbors(cornerNode, allowDiagonals: true);

        // Assert
        Assert.Equal(3, neighbors.Count); // (0,1), (1,0), (1,1)
    }
}
