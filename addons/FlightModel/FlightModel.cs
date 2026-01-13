#if TOOLS
using Godot;

[Tool]
public partial class FlightModel : EditorPlugin
{
	private FlightSurfaceGizmo gizmo;
	public override void _EnterTree()
	{
		Script script = GD.Load<Script>("res://addons/FlightModel/FlightSurface.cs");
		Texture2D texture = GD.Load<Texture2D>("res://icons/nodes/flight_surface_icon.png");
		AddCustomType("FlightSurface", "Node3D", script, texture);
		gizmo = (FlightSurfaceGizmo)GD.Load<CSharpScript>(
			"res://addons/FlightModel/FlightSurfaceGizmo.cs")
			.New(GetUndoRedo());
		AddNode3DGizmoPlugin(gizmo);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("FlightSurface");
		RemoveNode3DGizmoPlugin(gizmo);
	}
}
#endif
