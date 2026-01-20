using System;
using System.Linq;
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
    private const int DragHandle = 4;
    private const int LiftHandle = 5;

    private const string NodeMainMaterial = "NodeMainMaterial";
    private const string HandleMainPointMaterial = "HandleMainPointMaterial";
    private const string NodeLiftMaterial = "NodeLiftMaterial";
    private const string HandleNodeLiftPointMaterial = "HandleNodeLiftPointMaterial";
    private const string NodeVolumeMaterial = "NodeVolumeMaterial";

    public FlightSurfaceGizmo() : base()
    {
    }

    public FlightSurfaceGizmo(EditorUndoRedoManager undoRedo)
    {
        CreateMaterial(NodeMainMaterial, new Color(1, 0, 0));
        CreateMaterial(NodeLiftMaterial, new Color(0, 1, 0));
        CreateMaterial(NodeVolumeMaterial, new Color(0.5f, 0, 0, 0.25f));
        CreateHandleMaterial(HandleMainPointMaterial);
        CreateHandleMaterial(HandleNodeLiftPointMaterial);
        var handleMainPointMaterial = GetMaterial(HandleMainPointMaterial);
        var handleNodeLiftMaterial = GetMaterial(HandleNodeLiftPointMaterial);
        handleMainPointMaterial.AlbedoColor = new Color(1, 0, 0);
        handleNodeLiftMaterial.AlbedoColor = new Color(0, 1, 0);
        _undoRedo = undoRedo;
    }

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        base._Redraw(gizmo);
        gizmo.Clear();
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();

        // TODO: drawn off center when drawn for children experiencing parent transformations
        var center = flightSurface.Position;
        var front = flightSurface.GlobalBasis.Z * flightSurface.Length * 0.5f;
        var right = flightSurface.GlobalBasis.X * flightSurface.Width * 0.5f;
        var down = flightSurface.GlobalBasis.Y * flightSurface.DragCoefficient;
        var up = flightSurface.GlobalBasis.Y * flightSurface.LiftCoefficient;

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

        var dragLines = new[]
        {
            // drag line front
            center + right - front - down,
            center - right - front - down,
            // drag line right
            center + right - front,
            center + right - front - down,
            // drag line left
            center - right - front,
            center - right - front - down,
        };

        var liftLines = new[]
        {
            // lift line
            center,
            center + up
        };

        var handles = new[]
        {
            center + right,
            center - right,
            center + front,
            center - front,
            center - front - down // drag handle
        };

        var liftHandles = new[]
        {
            center + up
        };

        Mesh topMesh = CreateMesh(lines.Skip(4).ToArray());
        Mesh dragMesh = CreateMesh(dragLines.Skip(2).ToArray());

        gizmo.AddLines(lines, GetMaterial(NodeMainMaterial, gizmo));
        gizmo.AddMesh(topMesh, GetMaterial(NodeVolumeMaterial, gizmo), Transform3D.Identity);
        gizmo.AddLines(dragLines, GetMaterial(NodeMainMaterial, gizmo));
        gizmo.AddMesh(dragMesh, GetMaterial(NodeVolumeMaterial, gizmo), Transform3D.Identity);
        gizmo.AddLines(liftLines, GetMaterial(NodeLiftMaterial, gizmo));
        gizmo.AddHandles(handles, GetMaterial(HandleMainPointMaterial, gizmo), new[] { WidthHandlePos, WidthHandleNeg, LengthHandlePos, LengthHandleNeg, DragHandle });
        gizmo.AddHandles(liftHandles, GetMaterial(HandleNodeLiftPointMaterial, gizmo), new[] { LiftHandle });
    }

    public Mesh CreateMesh(Vector3[] points)
    {
        if (points.Length != 4)
            throw new ArgumentException("CreateMesh expects exactly 4 points");

        var vertices = new Vector3[]
        {
            points[0],
            points[1],
            points[2],
            points[3],
        };

        // Two triangles, CCW winding
        var indices = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };

        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);

        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        var mesh = new ArrayMesh();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        return mesh;
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
            case DragHandle:
                _undoRedo.AddDoProperty(flightSurface, FlightSurface.PropertyName.DragCoefficient, flightSurface.DragCoefficient);
                _undoRedo.AddUndoProperty(flightSurface, FlightSurface.PropertyName.DragCoefficient, restore);
                break;
            case LiftHandle:
                _undoRedo.AddDoProperty(flightSurface, FlightSurface.PropertyName.LiftCoefficient, flightSurface.LiftCoefficient);
                _undoRedo.AddUndoProperty(flightSurface, FlightSurface.PropertyName.LiftCoefficient, restore);
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
                case DragHandle:
                    flightSurface.DragCoefficient = (float)restore;
                    break;
                case LiftHandle:
                    flightSurface.LiftCoefficient = (float)restore;
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
            case DragHandle:
                return nameof(FlightSurface.DragCoefficient);
            case LiftHandle:
                return nameof(FlightSurface.LiftCoefficient);
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
            case DragHandle:
                return flightSurface.DragCoefficient;
            case LiftHandle:
                return flightSurface.LiftCoefficient;
            default:
                return base._GetHandleValue(gizmo, handleId, secondary);
        }
    }

    public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
    {
        FlightSurface flightSurface = (FlightSurface)gizmo.GetNode3D();
        var front = flightSurface.GlobalBasis.Z * flightSurface.Length * 0.5f;
        var right = flightSurface.GlobalBasis.X * flightSurface.Width * 0.5f;
        var down = flightSurface.GlobalBasis.Y * flightSurface.DragCoefficient;
        var up = flightSurface.GlobalBasis.Y * flightSurface.LiftCoefficient;

        // TODO: handles drag influence in global rather than local space regardless of local rotation
        // TODO: project along the line (length or width) in screen space and use 2 * distance to center for final length
        switch (handleId)
        {
            case LengthHandlePos:
                flightSurface.Length = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - right)).Z;
                break;
            case LengthHandleNeg:
                flightSurface.Length = -camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + right)).Z;
                break;
            case WidthHandlePos:
                flightSurface.Width = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition - front)).X;
                break;
            case WidthHandleNeg:
                flightSurface.Width = -camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + front)).X;
                break;
            case DragHandle:
                flightSurface.DragCoefficient = -camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition + front)).Y;
                break;
            case LiftHandle:
                flightSurface.LiftCoefficient = camera.ProjectPosition(
                    screenPos,
                    GetZDepth(
                        camera, flightSurface.GlobalPosition)).Y;
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