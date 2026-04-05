# Autonomous Path Simulator — Distributed Swarm Edition

A high-performance robotics pathfinding simulation built with .NET and Blazor Server. The project has evolved from a single-process simulation into a **distributed swarm architecture** — mirroring how real-world warehouse robot systems (like Amazon Kiva) operate.

## Architecture: "The Tower & The Fleet"

The system is split into three distinct roles communicating over **SignalR WebSockets** in real time:

```
┌──────────────────┐        SignalR        ┌──────────────────────┐
│  TractorDriver   │ ─── ReportPosition ──▶│  TrafficControl API  │
│  (The Fleet)     │ ◀── TractorJoined ─── │  (The Tower)         │
│  n instances     │                       │  port :5259          │
└──────────────────┘                       └──────────┬───────────┘
                                                      │ UpdateFleet
                                                      ▼
                                           ┌──────────────────────┐
                                           │  Visualizer          │
                                           │  (The Monitor)       │
                                           │  port :5206          │
                                           └──────────────────────┘
```

### Projects

| Project | Role | Type |
|---|---|---|
| `Grid` | Domain layer — coordinate mapping, node states | Class Library |
| `Pathfinder` | A* engine — optimal path calculation | Class Library |
| `EStop` | Safety layer — cooperative cancellation | Class Library |
| `Swarm.Protocol` | Shared data contracts (DTOs) between Tower and Fleet | Class Library |
| `TrafficControl` | The Tower — central SignalR hub, holds master fleet state | ASP.NET Core Web API |
| `TractorDriver` | The Fleet — autonomous agent, drives itself using A* | Console App |
| `Visualizer` | The Monitor — Blazor Server dashboard, renders live data | Blazor Server App |

---

## Key Features

### 1. Distributed Swarm via SignalR
The system uses **ASP.NET Core SignalR** WebSockets to coordinate the fleet in real time:
- `RegisterTractor`: A tractor signs in to the Tower when it starts.
- `ReportPosition`: Each tractor broadcasts its position every 500ms.
- `UpdateFleet`: The Tower fans out the full fleet state to all connected dashboards.
- `JoinDashboard`: The Visualizer subscribes to a dedicated group to receive live updates without spamming other tractors.

### 2. Autonomous Tractor Agents
Each `TractorDriver` instance is a self-contained agent that:
- Generates its own unique ID (Guid) on boot.
- Picks random destinations on a 20×20 grid.
- Calculates its own path using A* via the `Pathfinder` library.
- Reports each step to the Tower and rests 2 seconds between jobs.
- Reconnects automatically if the Tower goes offline.

Run multiple instances in parallel to simulate a real fleet.

### 3. Intelligent A* Pathfinding
The algorithm calculates the optimal route by weighing terrain difficulty:
- `Blocked`: Impassable obstacles (fences, buildings).
- `Restricted`: High-cost terrain (mud, sand) — the vehicle weighs detour cost vs. terrain penalty.
- `Octile Distance`: Optimized for 8-way movement; diagonal travel costs 1.41 vs. 1.0 straight.

### 4. Swarm Monitor Dashboard (`/swarm`)
A live Blazor page that acts as a "dumb terminal" — it only renders what the Tower sends:
- **Connection status** indicator with auto-retry if the Tower is offline.
- **Fleet statistics** — active tractors, moving, idle, blocked counts.
- **Live 20×20 grid** — tractor positions update in real time; collision warnings shown when multiple tractors share a cell.
- **Fleet details table** — per-tractor ID, coordinates, status badge, and battery level bar.

### 5. Single-Tractor Simulator (`/`)
The original interactive simulation remains on the home page:
- A* pathfinding from (0,0) to (14,9) on a 15×10 grid.
- Click tiles to toggle Traversable → Blocked → Restricted states.
- Live path recalculation when obstacles are placed on the active path.
- Real-time telemetry: position, G-Cost, H-Score, system status.

### 6. Safety-Critical Emergency Stop (E-Stop)
Modeled after real-world robotics protocols using `CancellationTokenSource`:
- **Immediate interruption**: `.Cancel()` stops the movement loop instantly.
- **Exception-driven safety**: `OperationCanceledException` ensures the vehicle never reaches an undefined state.
- **Thread-safe execution**: Path is copied before simulation to prevent collection modification errors.

---

## Tech Stack

| | |
|---|---|
| Language | C# / .NET 10 |
| UI Framework | Blazor Interactive Server |
| Real-Time Transport | ASP.NET Core SignalR (WebSockets) |
| Pathfinding | A* with Octile Distance |
| Testing | xUnit |
| Concurrency | `ConcurrentDictionary`, `CancellationToken` |

---

## Installation & Running

### Prerequisites
- .NET 10.0 or higher
- A modern web browser

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/EdwinTJ/Path-Simulation/
   cd Path-Simulation
   ```

2. **Run unit tests** (optional)
   ```bash
   dotnet test
   ```

3. **Start the Tower** (TrafficControl API)
   ```bash
   dotnet run --project TrafficControl
   # Listening on http://localhost:5259
   ```

4. **Launch the Visualizer** (in a second terminal)
   ```bash
   dotnet watch run --project Visualizer
   # Open http://localhost:5206
   ```

5. **Deploy the Fleet** (open one terminal per tractor — run as many as you want)
   ```bash
   dotnet run --project TractorDriver
   dotnet run --project TractorDriver
   dotnet run --project TractorDriver
   # Each instance is a unique autonomous tractor
   ```

---

## Usage

### Swarm Monitor (`/swarm`)
1. Start the **Tower** first, then open the Visualizer.
2. Navigate to **Swarm Monitor** in the nav menu.
3. The page auto-connects to the Tower — status shows green when live.
4. Start one or more **TractorDriver** instances to populate the fleet.
5. Watch tractors appear on the grid and move in real time.
6. Hover over a tractor icon to see its ID, battery level, and status.

### Single-Tractor Simulator (`/`)
1. Click **Start Simulation** to pathfind from (0,0) to (14,9).
2. Click any tile to toggle its state (Traversable → Blocked → Restricted).
3. Click **E-Stop** to halt the vehicle immediately.
4. Click **Reset** to clear the path and return to start.
