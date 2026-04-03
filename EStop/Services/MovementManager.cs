using System;
using System.Collections.Generic;
using System.Text;
using Grid.Models;

namespace EStop.Services;

public class MovementManager
{
    public event Action<Node>? OnVehivleMoved;

    public async Task StartSimulationAsync(List<Node> path, CancellationToken token)
    {
        try
        {
            foreach(var node in path)
            {
                // Stop before the next move
                token.ThrowIfCancellationRequested();

                // Update Position
                OnVehivleMoved?.Invoke(node);

                // Delay
                await Task.Delay(500, token);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("E-Stop: Vehivle Halted");
            throw;
        }
    }
}