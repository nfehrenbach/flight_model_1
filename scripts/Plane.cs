using Godot;
using System;
using FlightModel1.Models;
using System.Collections.Generic;

namespace FlightModel1;

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
	#endregion Exports

	protected Inputs? inputs;
	protected IEnumerable<Node3D> LiftSurfaces { get; set; } = Array.Empty<Node3D>();

	public void SetInputValues(Inputs inputValues)
	{
		inputs = inputValues;
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
		if (inputs != null)
		{
			RotateObjectLocal(Vector3.Right, (float)(PitchSensitivity * inputs.PitchAxis * delta));
			RotateObjectLocal(Vector3.Forward, (float)(RollSensitivity * inputs.RollAxis * delta));
			RotateObjectLocal(Vector3.Up, (float)(YawSensitivity * inputs.YawAxis * delta));
		}

		if (inputs != null && inputs.ThrottleAxis != 0)
		{
			ThrottlePercent += inputs.ThrottleAxis * ThrottleSensitivity;
			ThrottlePercent = Math.Clamp(ThrottlePercent, -100f, 100f);
			EmitSignal(SignalName.ThrottleChanged, ThrottlePercent);
		}

		SetDrag();
		SetVelocity();
	}

	private void SetVelocity()
	{
		Vector3 forward = -GlobalBasis.Z;
		Velocity += forward * Thrust * ThrottlePercent * ThrottleMultiplier;
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
