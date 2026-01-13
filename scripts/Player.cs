using Godot;
using System;
using FlightModel1.Models;
using FlightModel1.Utils;

namespace FlightModel1;

public partial class Player : Node3D
{
	[ExportCategory("Main Category")]
	[Export]
	public Plane Plane;

	private bool _idle = true;
	private float pitchInput = 0;
	private float rollInput = 0;
	private float yawInput = 0;
	private float throttleInput = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private Inputs ReadInput()
	{
		pitchInput = Input.GetAxis(Constants.AxisNames.PitchDown, Constants.AxisNames.PitchUp);
		rollInput = Input.GetAxis(Constants.AxisNames.RollLeft, Constants.AxisNames.RollRight);
		yawInput = -1f * Input.GetAxis(Constants.AxisNames.YawLeft, Constants.AxisNames.YawRight);

		throttleInput = (Input.IsActionPressed(Constants.AxisNames.ThrottleUp) ? 1f : 0f)
			- (Input.IsActionPressed(Constants.AxisNames.ThrottleDown) ? 1f : 0f);

		return new Inputs(
			rollInput,
			pitchInput,
			yawInput,
			throttleInput
		);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Plane.SetInputValues(ReadInput());
	}
}
