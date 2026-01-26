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

	protected override void MoveFlightSurfaces()
	{
		// Stabilators
		if (Util.IsAlive(leftStabilator) && Util.IsAlive(rightStabilator) && ControlInputs != null)
		{
			Basis rollBasis = new Basis(Vector3.Forward, Mathf.DegToRad(rollInput));
			leftStabilator.Transform = new Transform3D(_leftStabilatorRestBasis * ruleBasis, leftStabilator.Transform.Origin);
			// leftStabilator.RotationDegrees = new Vector3(rollInput + pitchInput, 0f, 0f);
			// rightStabilator.RotationDegrees = new Vector3(-rollInput + pitchInput, 0f, 0f);
		}

		// Rudders
		if (Util.IsAlive(leftRudder) && Util.IsAlive(rightRudder) && ControlInputs != null)
		{
			Basis yawBasis = new Basis(Vector3.Up, yawRad);
			leftRudder.Transform = new Transform3D(_leftRudderRestBasis * yawBasis, leftRudder.Transform.Origin);
			rightRudder.Transform = new Transform3D(_rightRudderRestBasis * yawBasis, rightRudder.Transform.Origin);
		}

		// Spoilers
		if (Util.IsAlive(rightInnerSpoiler))
		{

		}
	}
}
