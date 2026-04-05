using Microsoft.AspNetCore.SignalR.Client;
using Grid.Models;
using Grid.Services;
using Pathfinder.Services;
using Swarm.Protocol;
using Microsoft.AspNetCore.Connections;

var tractorId = Guid.NewGuid();
Console.WriteLine($"Tractor {tractorId} starting enfine...");

var connection = new HubConnectionBuilder()
                        .WithUrl("http://localhost:5259/swarmhub")
                        .WithAutomaticReconnect()
                        .Build();

await connection.StartAsync();
await connection.InvokeAsync("RegisterTractor", tractorId);

var grid = new GridManager(20,20);
var pathfinder = new Pathfinder.Services.Pathfinder(grid);
var random = new Random();

while (true)
{
    var start = grid.GetNode(1,0);
    var end = grid.GetNode(random.Next(0,20),random.Next(0,20));

    Console.WriteLine($"Calculating path to ({end.X}, {end.Y})...");

    var path = pathfinder.FindPath(start,end);

    foreach(var step in path)
    {
        await Task.Delay(500);

        var telemetry = new TractorState
        {
            Id = tractorId,
            X = step.X,
            Y = step.Y,
            Status = "Moving",
            BatteryLevel = 100
        };

        try
        {
            await connection.InvokeAsync("ReportPosition", telemetry);
            Console.WriteLine($"Reported: {step.X}, {step.Y}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Radio Silence: {ex.Message}");
        }
    }

    await Task.Delay(2000);
}