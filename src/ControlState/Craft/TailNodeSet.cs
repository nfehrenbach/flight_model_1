using Godot;

namespace FlightModel1.ControlState.Craft;

#nullable enable

public record TailNodeSet(
    Node3D? Elevator,
    Node3D? Rudder,
    Node3D? Stabilator
)
{ }