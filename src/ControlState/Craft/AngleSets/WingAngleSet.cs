

using System.Net.NetworkInformation;

namespace FlightModel1.ControlState.Craft.AngleSets;

#nullable enable

public class WingAngleSet
{
    public MaximumMinimumDegreeAngles Aileron { get; set; } = new();
    public MaximumMinimumDegreeAngles Flaps { get; set; } = new();
    public MaximumMinimumDegreeAngles InnerSpoiler { get; set; } = new();
    public MaximumMinimumDegreeAngles MiddleSpoiler { get; set; } = new();
    public MaximumMinimumDegreeAngles OuterSpoiler { get; set; } = new();
    public MaximumMinimumDegreeAngles Pivot { get; set; } = new();

    public WingAngleSet(
        MaximumMinimumDegreeAngles? aileron = null,
        MaximumMinimumDegreeAngles? flaps = null,
        MaximumMinimumDegreeAngles? innerSpoiler = null,
        MaximumMinimumDegreeAngles? middleSpoiler = null,
        MaximumMinimumDegreeAngles? outerSpoiler = null,
        MaximumMinimumDegreeAngles? pivot = null)
    {
        Aileron = aileron ?? new();
        Flaps = flaps ?? new();
        InnerSpoiler = innerSpoiler ?? new();
        MiddleSpoiler = middleSpoiler ?? new();
        OuterSpoiler = outerSpoiler ?? new();
        Pivot = pivot ?? new();
    }
}