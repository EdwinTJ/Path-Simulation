namespace Swarm.Protocol;

public class TractorState
{
    public Guid Id { get; set; } // License Plate
    public int X {get; set;} // Current X in Grid
    public int Y { get; set;} // Current Y in Grid
    public string Status {get; set;} 
    public float BatteryLevel {get; set;} 
}
