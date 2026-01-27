using System.Net.Http.Headers;
using Godot;

namespace FlightModel1.ControlState.Craft.AngleSets;

#nullable enable
public class InputAngleLimits
{
    public WingAngleSet LeftWing { get; set; } = new();
    public WingAngleSet RightWing { get; set; } = new();
    public TailAngleSet LeftTail { get; set; } = new();
    public TailAngleSet RightTail { get; set; } = new();
    public TailAngleSet DorsalTail { get; set; } = new();

    public InputAngleLimits(
        WingAngleSet? leftWing = null,
        WingAngleSet? rightWing = null,
        TailAngleSet? leftTail = null,
        TailAngleSet? rightTail = null,
        TailAngleSet? dorsalTail = null)
    {
        LeftWing = leftWing ?? new();
        RightWing = rightWing ?? new();
        LeftTail = leftTail ?? new();
        RightTail = rightTail ?? new();
        DorsalTail = dorsalTail ?? new();
    }
}