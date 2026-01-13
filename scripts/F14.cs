using Godot;
using System;
using FlightModel1.Models;

namespace FlightModel1;

#nullable enable

public partial class F14 : Plane
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print($"Test F14._Ready");
	}

	protected override void MoveFlightSurfaces()
	{

	}
}
