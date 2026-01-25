using Godot;
using System;
using FlightModel1.ControlState.Craft;
using System.Collections.Generic;

namespace FlightModel1.Scene.Craft;

#nullable enable

public abstract partial class Plane : CharacterBody3D
{
	#region Exports
	[ExportCategory("Plane Stats")]
	[Export]
	public float ThrottlePercent { get; set; } = 0.0f;

	[Export]
	public float ThrottleMultiplier { get; set; } = 0.01f;

	[Export]
	public float ThrottleSensitivity { get; set; } = 0.5f;

	[Export]
	public float YawSensitivity { get; set; } = 1.0f;

	[Export]
	public float RollSensitivity { get; set; } = 1.0f;

	[Export]
	public float PitchSensitivity { get; set; } = 1.0f;

	[Export]
	public float Thrust { get; set; } = 40f;

	[Export]
	public float MaxSpeed { get; set; } = 800f;

	[ExportCategory("Surface Limits")]
	[Export]
	public float MaxRollStabilatorAngle { get; set; } = 15f;

	[Export]
	public float MaxPitchStabilatorAngle { get; set; } = 15f;

	[Export]
	public float MaxRudderAngle { get; set; } = 30f;

	[Export]
	public float MaxPitchSpoilerAngle { get; set; } = 25f;
	#endregion Exports

	protected Inputs? CtrlInputs { get; set; } = null;
	protected IEnumerable<Node3D> LiftSurfaces { get; set; } = Array.Empty<Node3D>();
	protected float LateralThrustPercent { get; set; } = 0f;
	protected float VerticalThrustPercent { get; set; } = 0f;

	public void SetInputValues(Inputs inputValues)
	{
		CtrlInputs = inputValues;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateFlightModel((float)delta);
		MoveAndSlide();
		MoveFlightSurfaces();
		EmitSignal(SignalName.ElevationChanged, Position.Y);
		EmitSignal(SignalName.VelocityChanged, Velocity.Length());
	}

	protected abstract void MoveFlightSurfaces();

	private void UpdateFlightModel(double delta)
	{
		if (CtrlInputs != null)
		{
			RotateObjectLocal(Vector3.Right, (float)(PitchSensitivity * CtrlInputs.PitchAxis * delta));
			RotateObjectLocal(Vector3.Forward, (float)(RollSensitivity * CtrlInputs.RollAxis * delta));
			RotateObjectLocal(Vector3.Up, (float)(YawSensitivity * CtrlInputs.YawAxis * delta));
		}

		if (CtrlInputs != null && CtrlInputs.ThrottleAxis != 0)
		{
			ThrottlePercent += CtrlInputs.ThrottleAxis * ThrottleSensitivity;
			ThrottlePercent = Math.Clamp(ThrottlePercent, -100f, 100f);
			EmitSignal(SignalName.ThrottleChanged, ThrottlePercent);
		} // TODO: NEON-21 should be placed into helper function and unified with other thrust axes

		if (CtrlInputs != null)
		{
			LateralThrustPercent = CtrlInputs.StrafeLRAxis;
			VerticalThrustPercent = CtrlInputs.StrafeUDAxis;
		}

		SetDrag();
		SetVelocity();
	}

	private void SetVelocity()
	{
		Vector3 forward = -GlobalBasis.Z;
		Velocity += forward * Thrust * ThrottlePercent * ThrottleMultiplier;
		Vector3 right = GlobalBasis.X;
		Velocity += right * Thrust * ThrottleMultiplier * (LateralThrustPercent * 100.0f);
		Vector3 up = GlobalBasis.Y;
		Velocity += up * Thrust * ThrottleMultiplier * (VerticalThrustPercent * 100.0f);
		Velocity = Velocity.LimitLength(MaxSpeed);
	}

	private void SetDrag()
	{
		Vector3 DragVector = GetDragVector();
		Velocity *= DragVector;
	}

	private Vector3 GetDragVector()
	{
		return new Vector3(0.95f, 0.95f, 0.95f);
	}

	[Signal]
	public delegate void ThrottleChangedEventHandler(float throttlePercent);
	[Signal]
	public delegate void VelocityChangedEventHandler(float velocityMetersPerSecond);
	[Signal]
	public delegate void ElevationChangedEventHandler(float elevationMeters);
}
