namespace FlightModel1.Models;

#nullable enable

/// <summary>
/// Contains the needed input values to update a plane model with 6DOF
/// </summary>
/// <param name="RollAxis"></param>
/// <param name="PitchAxis"></param>
/// <param name="YawAxis"></param>
/// <param name="ThrottleAxis"></param>
/// <param name="StrafeLRAxis"></param>
/// <param name="StrafeUDAxis"></param>
public record Inputs(
    float RollAxis = 0f,
    float PitchAxis = 0f,
    float YawAxis = 0f,
    float ThrottleAxis = 0f,
    float StrafeLRAxis = 0f,
    float StrafeUDAxis = 0f
);