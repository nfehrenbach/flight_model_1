using Godot;

namespace FlightModel1.ControlState.Craft.BasisSets;

#nullable enable

public record TailBasisSet(
    Basis? Elevator,
    Basis? Rudder,
    Basis? Stabilator
)
{ }