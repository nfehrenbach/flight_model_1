using FlightModel1.Enum;
using Godot;

namespace FlightModel1.ControlState;

public record ActionRecord(
    string InputName,
    ActionState State,
    long LastActionTimeMs,
    float? Value = 0f);