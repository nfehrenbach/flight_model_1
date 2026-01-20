using System.Collections.Generic;
using FlightModel1.Enum;

namespace FlightModel1.ControlState;

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
    float StrafeUDAxis = 0f,
    ActionState LayerL = ActionState.None,
    ActionState LayerR = ActionState.None)
{
    public IEnumerable<ActionRecord> GetAllInputs()
    {
        yield return new ActionRecord(nameof(RollAxis), ActionState.None, RollAxis);
        yield return new ActionRecord(nameof(PitchAxis), ActionState.None, PitchAxis);
        yield return new ActionRecord(nameof(YawAxis), ActionState.None, YawAxis);
        yield return new ActionRecord(nameof(ThrottleAxis), ActionState.None, ThrottleAxis);
        yield return new ActionRecord(nameof(StrafeLRAxis), ActionState.None, StrafeLRAxis);
        yield return new ActionRecord(nameof(StrafeUDAxis), ActionState.None, StrafeUDAxis);
        yield return new ActionRecord(nameof(LayerL), LayerL);
        yield return new ActionRecord(nameof(LayerR), LayerR);
    }
}