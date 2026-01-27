
namespace FlightModel1.ControlState.Craft.AngleSets;

#nullable enable

public class TailAngleSet
{
    public MaximumMinimumDegreeAngles Elevator { get; set; }
    public MaximumMinimumDegreeAngles Rudder { get; set; }
    public MaximumMinimumDegreeAngles PitchStabilator { get; set; }
    public MaximumMinimumDegreeAngles RollStabilator { get; set; }

    public TailAngleSet(
        MaximumMinimumDegreeAngles? elevator = null,
        MaximumMinimumDegreeAngles? rudder = null,
        MaximumMinimumDegreeAngles? pitchStabilator = null,
        MaximumMinimumDegreeAngles? rollStabilator = null)
    {
        Elevator = elevator ?? new();
        Rudder = rudder ?? new();
        PitchStabilator = pitchStabilator ?? new();
        RollStabilator = rollStabilator ?? new();
    }
}