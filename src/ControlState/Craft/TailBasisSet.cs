using Godot;

namespace FlightModel1.ControlState.Craft;

#nullable enable

public record TailBasisSet(
    Basis? Elevator,
    Basis? Rudder,
    Basis? Stabilator
)
{ }