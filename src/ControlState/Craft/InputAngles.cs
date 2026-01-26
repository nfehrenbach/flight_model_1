using Godot;

namespace FlightModel1.ControlState.Craft;

public class InputAngles
{
    public WingAngleSet LeftWing { get; set; } = new();
    public WingAngleSet RightWing { get; set; } = new();
    public TailAngleSet LeftTail { get; set; } = new();
    public TailAngleSet RightTail { get; set; } = new();
    public TailAngleSet DorsalTail { get; set; } = new();
}