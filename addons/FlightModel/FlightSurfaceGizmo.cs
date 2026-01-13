using System.Runtime.CompilerServices;
using Godot;

[Tool]
public partial class FlightSurfaceGizmo : EditorNode3DGizmoPlugin
{
    public override string _GetGizmoName()
        => "FlightSurfaceGizmo";

    public override bool _HasGizmo(Node3D forNode3D)
        => forNode3D is FlightSurface;

    private EditorUndoRedoManager _undoRedo;

    private const int LengthHandlePos = 0;
    private const int LengthHandleNeg = 1;
    private const int WidthHandlePos = 2;
    private const int WidthHandleNeg = 3;

    private const string NodeMainPointMaterial = "NodeMainPointMaterial";
    private const string HandleMainPointMaterial = "HandleMainPointMaterial";

    public FlightSurfaceGizmo(EditorUndoRedoManager undoRedo)
    {
        CreateMaterial(NodeMainPointMaterial, new Color(1, 0, 0));
        CreateHandleMaterial(HandleMainPointMaterial);
        var handleMainPointMaterial = GetMaterial(HandleMainPointMaterial);
        handleMainPointMaterial.AlbedoColor = new Color(1, 0, 0);
        _undoRedo = undoRedo;
    }

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        base._Redraw(gizmo);
        gizmo.Clear();
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();

        var center = flightSurface.Position;
        var front = flightSurface.GlobalBasis.X * flightSurface.Width * 0.5f;
        var right = flightSurface.GlobalBasis.Z * flightSurface.Length * 0.5f;

        var lines = new[]
        {
            // back side
            center - right - front,
            center + right - front,
            // front side
            center + right + front,
            center - right + front,
            // right side
            center + right - front,
            center + right + front,
            // left side
            center - right - front,
            center - right + front
        };

        var handles = new[]
        {
            center + right,
            center - right,
            center + front,
            center - front
        };

        gizmo.AddLines(lines, GetMaterial(NodeMainPointMaterial, gizmo));
        gizmo.AddHandles(handles, GetMaterial(HandleMainPointMaterial, gizmo), new[] { LengthHandlePos });
    }

    public override string _GetHandleName(EditorNode3DGizmo gizmo, int handleId, bool secondary)
    {
        switch (handleId)
        {
            case LengthHandlePos:
            case LengthHandleNeg:
                return nameof(FlightSurface.Length);
            case WidthHandlePos:
            case WidthHandleNeg:
                return nameof(FlightSurface.Width);
            default:
                return base._GetHandleName(gizmo, handleId, secondary);
        }
    }

    public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
    {
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();
        var front = flightSurface.GlobalBasis.X * flightSurface.Width * 0.5f;
        var right = flightSurface.GlobalBasis.Z * flightSurface.Length * 0.5f;
        switch (handleId)
        {
            case LengthHandlePos:
                flightSurface.Length = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + right)).Length();
                break;
            case LengthHandleNeg:
                flightSurface.Length = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - right)).Length();
                break;
            case WidthHandlePos:
                flightSurface.Length = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + front)).Length();
                break;
            case WidthHandleNeg:
                flightSurface.Length = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - front)).Length();
                break;
            default:
                base._SetHandle(gizmo, handleId, secondary, camera, screenPos);
                break;
        }
    }

    private float GetZDepth(Camera3D camera, Vector3 position)
    {
        Vector3 cameraPos = camera.GlobalPosition;
        Vector3 cameraFwd = -camera.GlobalTransform.Basis.Z;
        Vector3 vectorToPos = position - cameraPos;
        float zDepth = vectorToPos.Dot(cameraFwd);
        return zDepth;
    }
}