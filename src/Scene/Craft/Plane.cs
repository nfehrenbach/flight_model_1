using Godot;
using System;
using System.Collections.Generic;
using FlightModel1.ControlState.Craft;
using FlightModel1.Utils;

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
	float MaxPitchSpoilerAngle { get; set; } = 25f;
	#endregion Exports

	protected Inputs? ControlInputs { get; set; } = null;
	protected InputAngles InputAngleValues { get; set; } = new();
	protected IEnumerable<Node3D> LiftSurfaces { get; set; } = Array.Empty<Node3D>();
	protected float LateralThrustPercent { get; set; } = 0f;
	protected float VerticalThrustPercent { get; set; } = 0f;

	#region Model Node References
	protected InitialBasisSet? InitialBasisSet;
	protected FlightSurfaceNodeSet? FlightSurfaceNodes;
	#endregion

	protected Node3D? gRoot = null;

	/// <summary>
	/// Sets the FSNodes object
	/// </summary>
	/// <exception cref="ArgumentNullException"></exception>
	private void SetFSNodes()
	{
		if (gRoot == null)
			throw new ArgumentNullException(nameof(gRoot), "gRoot is null");
		FlightSurfaceNodes = new FlightSurfaceNodeSet(
			LeftWing: new WingNodeSet(
				Aileron: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftAileronStr),
				Flaps: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftFlapsStr),
				InnerSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftInnerSpoilerStr),
				MiddleSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftMiddleSpoilerStr),
				OuterSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftOuterSpoilerStr),
				Pivot: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftWingStr)
			),
			RightWing: new WingNodeSet(
				Aileron: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightAileronStr),
				Flaps: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightFlapsStr),
				InnerSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightInnerSpoilerStr),
				MiddleSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightMiddleSpoilerStr),
				OuterSpoiler: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightOuterSpoilerStr),
				Pivot: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightWingStr)
			),
			LeftTail: new TailNodeSet(
				Elevator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftElevatorStr),
				Rudder: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftRudderStr),
				Stabilator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.LeftStabilatorStr)
			),
			RightTail: new TailNodeSet(
				Elevator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightElevatorStr),
				Rudder: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightRudderStr),
				Stabilator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RightStabilatorStr)),
			DorsalTail: new TailNodeSet(
				Elevator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.ElevatorStr),
				Rudder: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.RudderStr),
				Stabilator: gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.StabilatorStr)
			)
		);
	}

	/// <summary>
	/// This sets the initial basis set after initialization of the nodes for rotating the flight surfaces
	/// </summary>
	/// <exception cref="ArgumentNullException"></exception>
	void SetInitialBasisSet()
	{
		if (FlightSurfaceNodes == null)
			throw new ArgumentNullException(nameof(FlightSurfaceNodes), "FSNodes is null");
		InitialBasisSet = new InitialBasisSet(
			LeftWing: new WingBasisSet(
				Aileron: FlightSurfaceNodes.LeftWing.Aileron?.Transform.Basis,
				Flaps: FlightSurfaceNodes.LeftWing.Flaps?.Transform.Basis,
				InnerSpoiler: FlightSurfaceNodes.LeftWing.InnerSpoiler?.Transform.Basis,
				MiddleSpoiler: FlightSurfaceNodes.LeftWing.MiddleSpoiler?.Transform.Basis,
				OuterSpoiler: FlightSurfaceNodes.LeftWing.OuterSpoiler?.Transform.Basis),
			RightWing: new WingBasisSet(
				Aileron: FlightSurfaceNodes.RightWing.Aileron?.Transform.Basis,
				Flaps: FlightSurfaceNodes.RightWing.Flaps?.Transform.Basis,
				InnerSpoiler: FlightSurfaceNodes.RightWing.InnerSpoiler?.Transform.Basis,
				MiddleSpoiler: FlightSurfaceNodes.RightWing.MiddleSpoiler?.Transform.Basis,
				OuterSpoiler: FlightSurfaceNodes.RightWing.OuterSpoiler?.Transform.Basis),
			LeftTail: new TailBasisSet(
				Elevator: FlightSurfaceNodes.LeftTail.Elevator?.Transform.Basis,
				Rudder: FlightSurfaceNodes.LeftTail.Rudder?.Transform.Basis,
				Stabilator: FlightSurfaceNodes.LeftTail.Stabilator?.Transform.Basis),
			RightTail: new TailBasisSet(
				Elevator: FlightSurfaceNodes.RightTail.Elevator?.Transform.Basis,
				Rudder: FlightSurfaceNodes.RightTail.Rudder?.Transform.Basis,
				Stabilator: FlightSurfaceNodes.RightTail.Stabilator?.Transform.Basis),
			DorsalTail: new TailBasisSet(
				Elevator: FlightSurfaceNodes.DorsalTail.Elevator?.Transform.Basis,
				Rudder: FlightSurfaceNodes.DorsalTail.Rudder?.Transform.Basis,
				Stabilator: FlightSurfaceNodes.DorsalTail.Stabilator?.Transform.Basis)
		);
	}

	private void InitializedInputAngles()
	{

	}

	public override void _Ready()
	{
		base._Ready();
		try
		{
			SetFSNodes();
			SetInitialBasisSet();
		}
		catch (ArgumentNullException e)
		{
			GD.PrintErr($"{e.Message}");
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error getting nodes: {e}");
		}
	}


	public void SetInputValues(Inputs inputValues)
	{
		ControlInputs = inputValues;
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

	protected virtual void MoveFlightSurfaces()
	{
		MoveRollSurfaces();
		MoveYawSurfaces();
		MovePitchSurfaces();
		MoveVectoringSurfaces();
	}

	protected virtual void MoveRollSurfaces()
	{
		if (ControlInputs == null || FlightSurfaceNodes == null)
			return;

		Basis rollBasis = new Basis(Vector3.Forward, Mathf.DegToRad(rollInput));

		if (Util.IsAlive(FlightSurfaceNodes.LeftTail.Stabilator))
			leftStabilator.Transform = new Transform3D(InitialBasisSet.LeftStabilatorRestBasis * rollBasis, leftStabilator.Transform.Origin);

		if (Util.IsAlive(rightStabilator))
			rightStabilator.Transform = new Transform3D(_rightStabilatorRestBasis * rollBasis, rightStabilator.Transform.Origin);
	}

	protected virtual void MoveYawSurfaces()
	{
		if (Util.IsAlive(leftRudder) && Util.IsAlive(rightRudder) && ControlInputs != null)
		{
			Basis yawBasis = new Basis(Vector3.Up, yawRad);
			leftRudder.Transform = new Transform3D(_leftRudderRestBasis * yawBasis, leftRudder.Transform.Origin);
			rightRudder.Transform = new Transform3D(_rightRudderRestBasis * yawBasis, rightRudder.Transform.Origin);
		}
	}

	protected virtual void MovePitchSurfaces()
	{
		if (Util.IsAlive(leftFlap) && Util.IsAlive(rightFlap) && ControlInputs != null)
		{
			Basis pitchBasis = new Basis(Vector3.Right, Mathf.DegToRad(pitchInput));
			leftFlap.Transform = new Transform3D(_leftFlapRestBasis * pitchBasis, leftFlap.Transform.Origin);
			rightFlap.Transform = new Transform3D(_rightFlapRestBasis * pitchBasis, rightFlap.Transform.Origin);
		}
	}

	protected virtual void MoveVectoringSurfaces()
	{

	}

	private void UpdateFlightModel(double delta)
	{
		if (ControlInputs != null)
		{
			RotateObjectLocal(Vector3.Right, (float)(PitchSensitivity * ControlInputs.PitchAxis * delta));
			RotateObjectLocal(Vector3.Forward, (float)(RollSensitivity * ControlInputs.RollAxis * delta));
			RotateObjectLocal(Vector3.Up, (float)(YawSensitivity * ControlInputs.YawAxis * delta));
		}

		if (ControlInputs != null && ControlInputs.ThrottleAxis != 0)
		{
			ThrottlePercent += ControlInputs.ThrottleAxis * ThrottleSensitivity;
			ThrottlePercent = Math.Clamp(ThrottlePercent, -100f, 100f);
			EmitSignal(SignalName.ThrottleChanged, ThrottlePercent);
		} // TODO: NEON-21 should be placed into helper function and unified with other thrust axes

		if (ControlInputs != null)
		{
			LateralThrustPercent = ControlInputs.StrafeLRAxis;
			VerticalThrustPercent = ControlInputs.StrafeUDAxis;
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
