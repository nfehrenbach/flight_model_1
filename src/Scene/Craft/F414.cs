using Godot;
using System;
using System.Collections.Generic;

namespace FlightModel1.Scene.Craft;

public partial class F414 : Plane
{
	private const string rootStr = "f14_Armature/fuselage";
	private const string leftWingStr = $"{rootStr}/receiver_wing_mount_l/socket_wing_mount_l/wing_pivot_mount_l/hinge_wing_l";
	private const string rightWingStr = $"{rootStr}/receiver_wing_mount_r/socket_wing_mount_r/wing_pivot_mount_r/hinge_wing_r";
	private const string leftWingSurfStr = $"{leftWingStr}/wing_l";
	private const string rightWingSurfStr = $"{rightWingStr}/wing_r";
	private const string leftFlapsStr = $"{leftWingSurfStr}/hinge_flaps_l";
	private const string rightFlapsStr = $"{rightWingSurfStr}/hinge_flaps_r";
	private const string leftInnerSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_inner_l";
	private const string rightInnerSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_inner_r";
	private const string leftOuterSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_outer_l";
	private const string rightOuterSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_outer_r";
	private const string leftMiddleSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_middle_l";
	private const string rightMiddleSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_middle_r";
	private const string leftStabilatorStr = $"{rootStr}/receiver_stabilator_strut_l/socket_stabilator_strut_l/hinge_stabilator_l";
	private const string rightStabilatorStr = $"{rootStr}/receiver_stabilator_strut_r/socket_stabilator_strut_r/hinge_stabilator_r";
	private const string leftRudderStr = $"{rootStr}/receiver_vstab_l/socket_vstab_l/hinge_rudder_l";
	private const string rightRudderStr = $"{rootStr}/receiver_vstab_r/socket_vstab_r/hinge_rudder_r";

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


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		leftWingPivot = GetNode<Node3D>(leftWingStr);
		rightWingPivot = GetNode<Node3D>(rightWingStr);
		leftFlap = GetNode<Node3D>(leftFlapsStr);
		rightFlap = GetNode<Node3D>(rightFlapsStr);
		leftInnerSpoiler = GetNode<Node3D>(leftInnerSpoilerStr);
		rightInnerSpoiler = GetNode<Node3D>(rightInnerSpoilerStr);
		leftOuterSpoiler = GetNode<Node3D>(leftOuterSpoilerStr);
		rightOuterSpoiler = GetNode<Node3D>(rightOuterSpoilerStr);
		leftMiddleSpoiler = GetNode<Node3D>(leftMiddleSpoilerStr);
		rightMiddleSpoiler = GetNode<Node3D>(rightMiddleSpoilerStr);
		leftStabilator = GetNode<Node3D>(leftStabilatorStr);
		rightStabilator = GetNode<Node3D>(rightStabilatorStr);
		leftRudder = GetNode<Node3D>(leftRudderStr);
		rightRudder = GetNode<Node3D>(rightRudderStr);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	protected override void MoveFlightSurfaces()
	{
	}
}
