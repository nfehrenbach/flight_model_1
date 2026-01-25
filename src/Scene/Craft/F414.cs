using Godot;
using System;
using System.Collections.Generic;
using FlightModel1.Utils;

namespace FlightModel1.Scene.Craft;

public partial class F414 : Plane
{
	private Node3D gRoot;
	private Node3D leftWingPivot;
	private Node3D rightWingPivot;
	private Node3D leftFlap;
	private Node3D rightFlap;
	private Node3D leftInnerSpoiler;
	private Node3D rightInnerSpoiler;
	private Node3D leftOuterSpoiler;
	private Node3D rightOuterSpoiler;
	private Node3D leftMiddleSpoiler;
	private Node3D rightMiddleSpoiler;
	private Node3D leftStabilator;
	private Node3D rightStabilator;
	private Node3D leftRudder;
	private Node3D rightRudder;
	private const string rootString = "f14_Armature";

	private Basis _leftRudderRestBasis;
	private Basis _leftInnerSpoilerRestBasis;
	private Basis _leftMiddleSpoilerRestBasis;
	private Basis _leftOuterSpoilerRestBasis;
	private Basis _leftFlapsRestBasis;
	private Basis _rightRudderRestBasis;
	private Basis _rightInnerSpoilerRestBasis;
	private Basis _rightMiddleSpoilerRestBasis;
	private Basis _rightOuterSpoilerRestBasis;
	private Basis _rightFlapsRestBasis;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		try
		{
			gRoot = GetNode<Node3D>(rootString);
			leftWingPivot = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftWingStr);
			rightWingPivot = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightWingStr);
			leftFlap = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftFlapsStr);
			rightFlap = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightFlapsStr);
			leftInnerSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftInnerSpoilerStr);
			_leftInnerSpoilerRestBasis = leftInnerSpoiler.Transform.Basis;
			leftOuterSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftOuterSpoilerStr);
			_leftOuterSpoilerRestBasis = leftOuterSpoiler.Transform.Basis;
			leftMiddleSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftMiddleSpoilerStr);
			_leftMiddleSpoilerRestBasis = leftMiddleSpoiler.Transform.Basis;
			leftStabilator = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftStabilatorStr);
			leftRudder = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.leftRudderStr);
			_leftRudderRestBasis = leftRudder.Transform.Basis;
			rightInnerSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightInnerSpoilerStr);
			_rightInnerSpoilerRestBasis = rightInnerSpoiler.Transform.Basis;
			rightMiddleSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightMiddleSpoilerStr);
			_rightMiddleSpoilerRestBasis = rightMiddleSpoiler.Transform.Basis;
			rightOuterSpoiler = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightOuterSpoilerStr);
			_rightOuterSpoilerRestBasis = rightOuterSpoiler.Transform.Basis;
			rightStabilator = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightStabilatorStr);
			rightRudder = gRoot.GetNode<Node3D>(Consts.PlaneGeoAddr.rightRudderStr);
			_rightRudderRestBasis = rightRudder.Transform.Basis;
		}
		catch (Exception e)
		{
			GD.PrintErr($"F414: Error getting nodes: {e}");
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
		if (Util.IsAlive(leftStabilator) && Util.IsAlive(rightStabilator) && CtrlInputs != null)
		{
			float rollInput = CtrlInputs.RollAxis * MaxRollStabilatorAngle;
			float pitchInput = -(CtrlInputs.PitchAxis * MaxPitchStabilatorAngle);
			leftStabilator.RotationDegrees = new Vector3(rollInput + pitchInput, 0f, 0f);
			rightStabilator.RotationDegrees = new Vector3(-rollInput + pitchInput, 0f, 0f);
		}

		// Rudders
		if (Util.IsAlive(leftRudder) && Util.IsAlive(rightRudder) && CtrlInputs != null)
		{
			float yawInput = -(CtrlInputs.YawAxis * MaxRudderAngle);
			float yawRad = Mathf.DegToRad(yawInput);
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
