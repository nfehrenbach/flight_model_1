using Godot;
using System;
using System.Collections.Generic;
using FlightModel1.ControlState.Craft;
using FlightModel1.ControlState.Craft.AngleSets;
using FlightModel1.ControlState.Craft.BasisSets;
using FlightModel1.ControlState.Craft.NodeSets;
using FlightModel1.Utils;

namespace FlightModel1.Scene.Craft;

#nullable enable

public abstract partial class Plane : CharacterBody3D
{
	#region Exports
	[ExportCategory("Plane Stats")]
	[ExportGroup("Throttle")]
	[Export]
	public float ThrottlePercent { get; set; } = 0.0f;

	[Export]
	public float ThrottleMultiplier { get; set; } = 0.01f;

	[Export]
	public float ThrottleSensitivity { get; set; } = 0.5f;

	[Export]
	public float Thrust { get; set; } = 40f;

	[Export]
	public float MaxSpeed { get; set; } = 800f;

	[ExportGroup("Manuevering")]
	[Export]
	public float YawSensitivity { get; set; } = 1.0f;

	[Export]
	public float RollSensitivity { get; set; } = 1.0f;

	[Export]
	public float PitchSensitivity { get; set; } = 1.0f;

	[ExportCategory("Surface Limits")]
	[ExportGroup("Wing Surfaces")]
	[Export]
	public float MaxAileronAngle { get; set; } = 30f;
	[Export]
	public float MinAileronAngle { get; set; } = -30f;
	[Export]
	public float MaxFlapsAngle { get; set; } = 30f;
	[Export]
	public float MinFlapsAngle { get; set; } = -30f;
	[Export]
	public float MaxInnerSpoilerAngle { get; set; } = 30f;
	[Export]
	public float MinInnerSpoilerAngle { get; set; } = -30f;
	[Export]
	public float MaxMiddleSpoilerAngle { get; set; } = 30f;
	[Export]
	public float MinMiddleSpoilerAngle { get; set; } = -30f;
	[Export]
	public float MaxOuterSpoilerAngle { get; set; } = 30f;
	[Export]
	public float MinOuterSpoilerAngle { get; set; } = -30f;
	[Export]
	public float MaxPivotAngle { get; set; } = 30f;
	[Export]
	public float MinPivotAngle { get; set; } = -30f;
	[ExportGroup("Tail Surfaces")]
	[Export]
	public float MaxElevatorAngle { get; set; } = 30f;
	[Export]
	public float MinElevatorAngle { get; set; } = -30f;
	[Export]
	public float MaxRudderAngle { get; set; } = 30f;
	[Export]
	public float MinRudderAngle { get; set; } = 30f;
	[Export]
	public float MaxRollStabilatorAngle { get; set; } = 30f;
	[Export]
	public float MinRollStabilatorAngle { get; set; } = -30f;
	[Export]
	public float MaxPitchStabilatorAngle { get; set; } = 30f;
	[Export]
	public float MinPitchStabilatorAngle { get; set; } = -30f;
	#endregion Exports

	protected Inputs? ControlInputs { get; set; } = null;
	protected IEnumerable<Node3D> LiftSurfaces { get; set; } = Array.Empty<Node3D>();
	protected float LateralThrustPercent { get; set; } = 0f;
	protected float VerticalThrustPercent { get; set; } = 0f;

	#region Model Node References
	protected InitialBasisSet? InitialBasisSet;
	protected FlightSurfaceNodeSet? FlightSurfaceNodes;
	protected InputAngleLimits? InputAngleLimits;
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

	private void InitializeInputAngleLimits()
	{
		InputAngleLimits = new InputAngleLimits(
			leftWing: new WingAngleSet(
				aileron: new(MaxAileronAngle, MinAileronAngle),
				flaps: new(MaxFlapsAngle, MinFlapsAngle),
				innerSpoiler: new(MaxInnerSpoilerAngle, MinInnerSpoilerAngle),
				middleSpoiler: new(MaxMiddleSpoilerAngle, MinMiddleSpoilerAngle),
				outerSpoiler: new(MaxOuterSpoilerAngle, MinOuterSpoilerAngle),
				pivot: new(MaxPivotAngle, MinPivotAngle)
			),
			rightWing: new WingAngleSet(
				aileron: new(MaxAileronAngle, MinAileronAngle),
				flaps: new(MaxFlapsAngle, MinFlapsAngle),
				innerSpoiler: new(MaxInnerSpoilerAngle, MinInnerSpoilerAngle),
				middleSpoiler: new(MaxMiddleSpoilerAngle, MinMiddleSpoilerAngle),
				outerSpoiler: new(MaxOuterSpoilerAngle, MinOuterSpoilerAngle),
				pivot: new(MaxPivotAngle, MinPivotAngle)
			),
			leftTail: new TailAngleSet(
				elevator: new(MaxElevatorAngle, MinElevatorAngle),
				rudder: new(MaxRudderAngle, MinRudderAngle),
				pitchStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle),
				rollStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle)
			),
			rightTail: new TailAngleSet(
				elevator: new(MaxElevatorAngle, MinElevatorAngle),
				rudder: new(MaxRudderAngle, MinRudderAngle),
				pitchStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle),
				rollStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle)
			),
			dorsalTail: new TailAngleSet(
				elevator: new(MaxElevatorAngle, MinElevatorAngle),
				rudder: new(MaxRudderAngle, MinRudderAngle),
				pitchStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle),
				rollStabilator: new(MaxRollStabilatorAngle, MinRollStabilatorAngle)
			)
		);
	}

	public override void _Ready()
	{
		base._Ready();
		try
		{
			SetFSNodes();
			SetInitialBasisSet();
			InitializeInputAngleLimits();
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

	private void CheckRotateByBasis(
		Node3D? flightSurfaceNode,
		Basis? initialBasis,
		MaximumMinimumDegreeAngles rotationLimit,
		float axisInput,
		Vector3? rotationAxis = null)
	{
		if (!Util.IsAlive(flightSurfaceNode) || initialBasis == null)
			return;

		rotationAxis ??= Vector3.Up;
		Basis newBasis = new Basis(rotationAxis.Value, Mathf.DegToRad(rotationLimit.FindLinearValueAlongAxis(axisInput)));
		flightSurfaceNode.Transform = new Transform3D(initialBasis.Value * newBasis, flightSurfaceNode.Transform.Origin);
	}

	protected virtual void MoveRollSurfaces()
	{
		if (ControlInputs == null || FlightSurfaceNodes == null || InitialBasisSet == null || InputAngleLimits == null)
			return;

		CheckRotateByBasis(
			FlightSurfaceNodes.LeftWing.Aileron,
			InitialBasisSet.LeftWing.Aileron,
			InputAngleLimits.LeftWing.Aileron,
			ControlInputs.RollAxis,
			Vector3.Left);
		CheckRotateByBasis(
			FlightSurfaceNodes.RightWing.Aileron,
			InitialBasisSet.RightWing.Aileron,
			InputAngleLimits.RightWing.Aileron,
			ControlInputs.RollAxis,
			Vector3.Right);
		CheckRotateByBasis(
			FlightSurfaceNodes.LeftTail.Stabilator,
			InitialBasisSet.LeftTail.Stabilator,
			InputAngleLimits.LeftTail.RollStabilator,
			ControlInputs.RollAxis,
			Vector3.Left);
		CheckRotateByBasis(
			FlightSurfaceNodes.RightTail.Stabilator,
			InitialBasisSet.RightTail.Stabilator,
			InputAngleLimits.RightTail.RollStabilator,
			ControlInputs.RollAxis,
			Vector3.Right);

	}

	protected virtual void MoveYawSurfaces()
	{
	}

	protected virtual void MovePitchSurfaces()
	{
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
