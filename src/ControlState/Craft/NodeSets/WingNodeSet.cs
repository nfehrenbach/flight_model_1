using Godot;

namespace FlightModel1.ControlState.Craft.NodeSets;

#nullable enable

public record WingNodeSet(
    Node3D? Aileron,
    Node3D? Flaps,
    Node3D? InnerSpoiler,
    Node3D? MiddleSpoiler,
    Node3D? OuterSpoiler,
    Node3D? Pivot
)
{ }