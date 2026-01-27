using Godot;
using System;
using FlightModel1.ControlState.UserActions;
using FlightModel1.ControlState.Craft;
using FlightModel1.Utils;
using FlightModel1.Scene.Craft;
using System.Diagnostics;

namespace FlightModel1.Scene.Player;

public partial class Player : Node3D
{
	[ExportCategory("Main Category")]
	[Export]
	public Craft.Plane Plane;

	private bool IsUsingLAndRStrafe = false;
	private ActionHistory actionHistory = new();
	Stopwatch loggingStopwatch;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Util.IsAlive(Plane))
			GD.PrintErr("Player: Plane is NOT assigned!");
		loggingStopwatch = Stopwatch.StartNew();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Plane.SetInputValues(ReadInput());
	}

	private Inputs ReadInput()
	{
		float ax1 = Input.GetAxis(Consts.AxisNames.Axis1Down, Consts.AxisNames.Axis1Up);
		float ax2 = Input.GetAxis(Consts.AxisNames.Axis2Down, Consts.AxisNames.Axis2Up);
		float ax3 = Input.GetAxis(Consts.AxisNames.Axis3Down, Consts.AxisNames.Axis3Up);
		float ax4 = Input.GetAxis(Consts.AxisNames.Axis4Down, Consts.AxisNames.Axis4Up);
		float ax5 = Input.GetAxis(Consts.AxisNames.Axis5Down, Consts.AxisNames.Axis5Up);
		float ax6 = (Input.IsActionPressed(Consts.AxisNames.Axis6Up) ? 1f : 0f)
			- (Input.IsActionPressed(Consts.AxisNames.Axis6Down) ? 1f : 0f);

		if (CheckLayerLAndRPressed())
		{
			IsUsingLAndRStrafe = !IsUsingLAndRStrafe;
			GD.Print($"Player: Toggled L+R strafe mode to {IsUsingLAndRStrafe}");
		}

		Inputs craftInput;
		if (!IsUsingLAndRStrafe)
		{
			craftInput = new Inputs(
				RollAxis: ax2,
				PitchAxis: -ax1,
				YawAxis: -ax3,
				ThrottleAxis: ax6);
		}
		else
		{
			craftInput = new Inputs(
				RollAxis: ax6,
				PitchAxis: -ax4,
				YawAxis: -ax5,
				ThrottleAxis: ax3,
				StrafeLRAxis: ax2,
				StrafeUDAxis: ax1);
		}
		// TODO: NEON-24 Should be a call to utility once implemented
		if (loggingStopwatch.ElapsedMilliseconds > 1000)
		{
			GD.Print($": {craftInput}");
			loggingStopwatch.Restart();
		}

		return craftInput;
	}

	private bool CheckLayerLAndRPressed()
		=> Input.IsActionPressed(Consts.ButtonNames.LayerL) // LayerL is a layer button
			&& Input.IsActionJustPressed(Consts.ButtonNames.LayerR); // LayerR is a active button
}
