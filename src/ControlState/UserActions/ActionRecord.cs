using FlightModel1.Enum;
using Godot;

namespace FlightModel1.ControlState.UserActions;

// TODO: NEON-23 need to improve this in order to enable cool down communication
public record ActionRecord(
    string InputName,
    ActionState State,
    long LastActionTimeMs,
    float? Value = 0f);