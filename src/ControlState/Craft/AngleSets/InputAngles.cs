using Godot;

namespace FlightModel1.ControlState.Craft.AngleSets;

#nullable enable
public class InputAngles
{
    public WingAngleSet LeftWing { get; set; } = new();
    public WingAngleSet RightWing { get; set; } = new();
    public TailAngleSet LeftTail { get; set; } = new();
    public TailAngleSet RightTail { get; set; } = new();
    public TailAngleSet DorsalTail { get; set; } = new();

    public InputAngles()
    {
    }

    public InputAngles(
        WingAngleSet leftWing,
        WingAngleSet rightWing,
        TailAngleSet? LeftTail,
        TailAngleSet? RighTail,
        TailAngleSet dorsalTail)
    {

    }
}