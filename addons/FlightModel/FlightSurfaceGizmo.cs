using System;
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

        // TODO: drawn off center when drawn for children experiencing parent transformations
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
        gizmo.AddHandles(handles, GetMaterial(HandleMainPointMaterial, gizmo), new[] { LengthHandlePos, LengthHandleNeg, WidthHandlePos, WidthHandleNeg });
    }

    public override void _CommitHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Variant restore, bool cancel)
    {
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();
        _undoRedo.CreateAction("Change FlightSurface");
        switch (handleId)
        {
            case LengthHandlePos:
            case LengthHandleNeg:
                _undoRedo.AddDoProperty(flightSurface, FlightSurface.PropertyName.Length, flightSurface.Length);
                _undoRedo.AddUndoProperty(flightSurface, FlightSurface.PropertyName.Length, restore);
                break;
            case WidthHandlePos:
            case WidthHandleNeg:
                _undoRedo.AddDoProperty(flightSurface, FlightSurface.PropertyName.Width, flightSurface.Width);
                _undoRedo.AddUndoProperty(flightSurface, FlightSurface.PropertyName.Width, restore);
                break;
            default:
                base._CommitHandle(gizmo, handleId, secondary, restore, cancel);
                break;
        }

        if (cancel)
        {
            switch (handleId)
            {
                case LengthHandlePos:
                case LengthHandleNeg:
                    flightSurface.Length = (float)restore;
                    break;
                case WidthHandlePos:
                case WidthHandleNeg:
                    flightSurface.Width = (float)restore;
                    break;
                default:
                    base._CommitHandle(gizmo, handleId, secondary, restore, cancel);
                    break;
            }
        }
        _undoRedo.CommitAction();
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

    public override Variant _GetHandleValue(EditorNode3DGizmo gizmo, int handleId, bool secondary)
    {
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();
        switch (handleId)
        {
            case LengthHandlePos:
            case LengthHandleNeg:
                return flightSurface.Length;
            case WidthHandlePos:
            case WidthHandleNeg:
                return flightSurface.Width;
            default:
                return base._GetHandleValue(gizmo, handleId, secondary);
        }
    }

    public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
    {
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();
        var front = flightSurface.GlobalBasis.X * flightSurface.Width * 0.5f;
        var right = flightSurface.GlobalBasis.Z * flightSurface.Length * 0.5f;

        // TODO: handles drag influence in global rather than local space regardless of local rotation
        // TODO: project along the line (length or width) in screen space and use 2 * distance to center for final length
        switch (handleId)
        {
            case LengthHandlePos:
                flightSurface.Length = Math.Abs(camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - right)).Z);
                break;
            case LengthHandleNeg:
                flightSurface.Length = Math.Abs(camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + right)).Z);
                break;
            case WidthHandlePos:
                flightSurface.Width = Math.Abs(camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - front)).X);
                break;
            case WidthHandleNeg:
                flightSurface.Width = Math.Abs(camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + front)).X);
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