using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Swarm.Protocol;

namespace TrafficControl.Hubs;

public class SwarmHub : Hub
{
    // Storage for all active tractors
    private static ConcurrentDictionary<Guid, TractorState> _fleet = new();

    // Tractor Sign In
    public async Task RegisterTractor(Guid tractorId)
    {
        var initState = new TractorState
        {
            Id = tractorId,
            X = 0,
            Y = 0,
            Status = "Idle"
        };

        _fleet.TryAdd(tractorId, initState);

        // Notify Client a new tractor joined
        await Clients.All.SendAsync("TractorJoined", initState);
    }    

    // Tractor moves update
    public async Task ReportPosition(TractorState state)
    {
        if (_fleet.ContainsKey(state.Id))
        {
            _fleet[state.Id] = state;

            // Broadcast updated position to client
            await Clients.Group("Dashboard").SendAsync("UpdateFleet", _fleet.Values);
            
        }
    }

    // Subscribe dashboard to updates
    public async Task JoinDashboard()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId,"Dashboard");
    }

}