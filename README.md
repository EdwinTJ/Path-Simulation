# Autonomous Path Simulator

A high performance robotics pathfinding simulation buildt with .Net and Blazor Server. The project aim to demostrate the connection between web interface and logic.

## Project Architecture

Design using `Sepratation of Concerns` the system is split into four distinct layers to ensure the core pathfinding logic can be tested and deployed independently of the UI.

- `Grid`: The "Domain" layer. Manages coordinate mapping, node states(Traversable, Blocked, Restricted) and nehighbor detection.
- `Pathfinder`: The "Engine". Implement the A* algorithm using an optimized PriorityQueue and Octile distance metrics.
- `EStop`: The "Safty" layer. Implements the Coorperative Cancellation patter for zero latency behicle halts.
- `Visualizer`: The "Presentation" layer. A Blazor Server App that renders the simulation in real-time.

## Key Features
1. Intelligent A* Pathfinding
The algorithm doesn't just find a paht; it calculates the optimal route by weighing terrain difficulty
- `Blocked`: Impassable obstacles (Fences, Buildings).
- `Restricted`: High-cost terrain (Mud, Sand). The vehicle weighs the "cost" of driving through mud vs. taking a longer detour.
- `Octile Distance`: Optimized for 8-way movement, correctly calculating the cost of diagonal travel (1.41) vs. straight travel (1.0).

2. Safety-Critical Emergency Stop (E-Stop)
Modeled after real-world robotics protocols, the E-Stop uses a `CancellationTokenSource`.
- `Immediate Interruption`: Calling .Cancel() interrupts the Task.Delay and stops the movement loop instantly.
- `Exception-Driven Safety`: Uses `OperationCanceledException` to ensure the vehicle never reaches an "undefined state" after a stop signal.

3. Decoupled Logic & Testing
The system is built to be "headless." The pathfinder doesn't know the UI exists. This allowed for:
- `Automated Unit Testing`: Verified via xUnit to ensure the algorithm handles "walls" and "edge cases" before visual development began.
- `Scalability`: The logic libraries could be ported to a console-based controller or a desktop app without changing a single line of code.

## Tech Stack
- Language: C#/.NET
- UI Framework: Blazor Interactive Server
- Testing: xUnit
- Algorithms: A*Search, Octile Distance

## Installation & Run
1. Clone the repo
```bash
git clone https://github.com/EdwinTJ/Path-Simulation/
cd Path-Simulation
```
2. Run Unit Tests
```bash
dotnet test
```
3. Launch the Simulator
```bash
dotnet watch --project Visualizer
```


