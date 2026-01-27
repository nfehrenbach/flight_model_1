using Godot;
using System;
using System.Collections.Generic;
using FlightModel1.Utils;

namespace FlightModel1.Scene.Craft;

public partial class F414 : Plane
{
	private const string rootString = "f14_Armature";


	float rollInput => ControlInputs.RollAxis * MaxRollStabilatorAngle;
	float pitchInput => -(ControlInputs.PitchAxis * MaxPitchStabilatorAngle);
	float yawInput => -(ControlInputs.YawAxis * MaxRudderAngle);
	float yawRad => Mathf.DegToRad(yawInput);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		try
		{
			gRoot = GetNode<Node3D>(rootString);
			base._Ready();
		}
		catch (Exception e)
		{
			GD.PrintErr($"F414: Error getting root node: {e}");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
