using Godot;
using System;

namespace FlightModel1;

#nullable enable

public partial class UI : Control
{
	private ProgressBar? throttleBar;
	private Label? throttleLabel;
	private Label? VelocityLabel;
	private Label? ElevationLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		throttleBar = GetNode<ProgressBar>("VBoxScreen/HBoxBottom/VBoxThrottle/BarThrottle");
		throttleLabel = GetNode<Label>("VBoxScreen/HBoxBottom/VBoxThrottle/LabelThrottle");
		VelocityLabel = GetNode<Label>("VBoxScreen/HBoxTop/VelocityLabel");
		ElevationLabel = GetNode<Label>("VBoxScreen/HBoxTop/ElevationLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnThrottleChanged(float value)
	{
		if (Util.IsAlive(throttleBar))
		{
			if (value < 0)
			{
				throttleBar.FillMode = (int)ProgressBar.FillModeEnum.TopToBottom;
				throttleBar.Value = Math.Abs(value);
			}
			else
			{
				throttleBar.FillMode = (int)ProgressBar.FillModeEnum.BottomToTop;
				throttleBar.Value = value;
			}
		}

		if (Util.IsAlive(throttleLabel))
		{
			if (value < 0)
			{
				throttleLabel.Text = "Rev. Throttle";
			}
			else
			{
				throttleLabel.Text = "Fwd. Throttle";
			}
		}
	}

	public void OnElevationChanged(float value)
	{
		if (Util.IsAlive(ElevationLabel))
			ElevationLabel.Text = String.Format("{0:0.0} m", value);
	}

	public void OnVelocityChanged(float value)
	{
		if (Util.IsAlive(VelocityLabel))
			VelocityLabel.Text = String.Format("{0:0.0} m/s", value);
	}
}
