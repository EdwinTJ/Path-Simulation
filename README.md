# Autonomous Path Simulator

A high-performance robotics pathfinding simulation built with .NET and Blazor Server. The project demonstrates the seamless connection between a web interface and core pathfinding logic.

## Project Architecture

Designed using `Separation of Concerns`, the system is split into four distinct layers to ensure the core pathfinding logic can be tested and deployed independently of the UI:

- `Grid`: The "Domain" layer. Manages coordinate mapping, node states (Traversable, Blocked, Restricted), and neighbor detection.
- `Pathfinder`: The "Engine". Implements the A* algorithm using an optimized PriorityQueue and Octile distance metrics.
- `EStop`: The "Safety" layer. Implements the Cooperative Cancellation pattern for zero-latency vehicle halts.
- `Visualizer`: The "Presentation" layer. A Blazor Server App that renders the simulation in real-time.

## Key Features

1. **Intelligent A* Pathfinding**
The algorithm doesn't just find a path; it calculates the optimal route by weighing terrain difficulty:
- `Blocked`: Impassable obstacles (Fences, Buildings).
- `Restricted`: High-cost terrain (Mud, Sand). The vehicle weighs the "cost" of driving through restricted areas vs. taking a longer detour.
- `Octile Distance`: Optimized for 8-way movement, correctly calculating the cost of diagonal travel (1.41) vs. straight travel (1.0).

2. **Safety-Critical Emergency Stop (E-Stop)**
Modeled after real-world robotics protocols, the E-Stop uses a `CancellationTokenSource`:
- `Immediate Interruption`: Calling .Cancel() interrupts the Task.Delay and stops the movement loop instantly.
- `Exception-Driven Safety`: Uses `OperationCanceledException` to ensure the vehicle never reaches an "undefined state" after a stop signal.
- `Thread-Safe Execution`: Creates a copy of the path before simulation to prevent collection modification errors during runtime.

3. **Interactive Grid Manipulation**
Click on any tile during or before simulation to:
- Toggle between `Traversable`, `Blocked`, and `Restricted` states.
- Trigger automatic path recalculation when obstacles are placed on the active path.
- Real-time updates without interrupting the simulation flow.

4. **Real-Time Telemetry Dashboard**
Live monitoring of vehicle movement and pathfinding metrics:
- `Position`: Current X, Y coordinates on the grid.
- `G-Cost`: Cumulative movement cost from start to current position.
- `H-Score`: Heuristic distance to target destination.
- `System Status`: Live status updates (Ready, Moving, E-Stop Triggered, Path Recalculation).

5. **Decoupled Logic & Testing**
The system is built to be "headless." The pathfinder doesn't know the UI exists:
- `Automated Unit Testing`: Verified via xUnit to ensure the algorithm handles "walls" and "edge cases" before visual development began.
- `Scalability`: The logic libraries could be ported to a console-based controller or a desktop app without changing a single line of code.

## Tech Stack
- Language: C#/.NET
- UI Framework: Blazor Interactive Server
- Testing: xUnit
- Algorithms: A*Search, Octile Distance

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

3. **Launch the simulator**
   ```bash
   dotnet watch run --project Visualizer
   ```
   The application will start on `http://localhost:5206`

## Usage

1. **Start Simulation**: Click "Start Simulation" to begin pathfinding from the top-left (0, 0) to the bottom-right (14, 9).
2. **Modify the Grid**: Click any tile during simulation to toggle its state (Traversable → Blocked → Restricted → Traversable).
3. **E-Stop**: Click "E-Stop" to halt the vehicle immediately.
4. **Reset**: Click "Reset" to clear the path and vehicle position for a new simulation.
5. **Monitor Status**: Watch the telemetry dashboard for real-time position, costs, and system status updates.


