using Godot;

namespace FlightModel1.ControlState.Craft.BasisSets;

#nullable enable

public record WingBasisSet(
    Basis? Aileron,
    Basis? Flaps,
    Basis? InnerSpoiler,
    Basis? MiddleSpoiler,
    Basis? OuterSpoiler
)
{ }