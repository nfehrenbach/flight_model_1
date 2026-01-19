using Godot;
using System;

namespace FlightModel1;

public partial class F414 : Plane
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
