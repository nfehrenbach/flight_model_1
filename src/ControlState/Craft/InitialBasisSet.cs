using Godot;

namespace FlightModel1.ControlState.Craft;

#nullable enable

public record InitialBasisSet(
    WingBasisSet LeftWing,
    WingBasisSet RightWing,
    TailBasisSet LeftTail,
    TailBasisSet RightTail,
    TailBasisSet DorsalTail
)
{ }