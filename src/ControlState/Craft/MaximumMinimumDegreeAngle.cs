
using System;
using System.Drawing;
using Godot;

namespace FlightModel1.ControlState.Craft;

#nullable enable
public class MaximumMinimumDegreeAngles
{
    public float Maximum { get; set; } = 30f;
    public float Minimum { get; set; } = -30f;

    public MaximumMinimumDegreeAngles(float? maximum = null, float? minimum = null)
    {
        if (maximum != null)
            Maximum = (float)maximum;

        if (minimum != null)
            Minimum = (float)minimum;
    }

    public float MaxRad => Mathf.DegToRad(Maximum);
    public float MinRad => Mathf.DegToRad(Minimum);

    public float FindLinearValueAlongAxis(float axisValue) => ((Maximum - Minimum) * axisValue) + Minimum;
}