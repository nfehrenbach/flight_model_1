using Godot;

namespace FlightModel1.ControlState.Craft.NodeSets;

#nullable enable

public record FlightSurfaceNodeSet(
    WingNodeSet LeftWing,
    WingNodeSet RightWing,
    TailNodeSet LeftTail,
    TailNodeSet RightTail,
    TailNodeSet DorsalTail
)
{ }