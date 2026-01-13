using System.Diagnostics;
using System.Reflection;
using Godot;

#nullable enable

[Tool]
public partial class FlightSurface : Node3D
{
    [Export]
    public float Length
    {
        get => _length;
        set
        {
            _length = value;
            UpdateGizmos();
        }
    }
    private float _length = 1.0f;

    [Export]
    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            UpdateGizmos();
        }
    }
    private float _width = 1.0f;

    [Export] public float LiftCoefficient { get; set; } = 1.0f;
    [Export] public float DragCoefficient { get; set; } = 0.2f;
    [Export] public Vector3 LocalNormal { get; set; } = Vector3.Up;

    public float Area => Width * Length;

    // Editor feedback
    public override void _Process(double delta)
    {
        //TODO: expose calculations
    }

    // Compute projected area for wind or lift calculation
    public float ComputeProjectedArea(Vector3 windDir)
    {
        Vector3 normalWorld = GlobalTransform.Basis * LocalNormal;
        float exposure = Mathf.Max(0f, normalWorld.Dot(-windDir.Normalized()));
        return Area * exposure;
    }

    // Optional: compute lift vector
    public Vector3 ComputeLift(Vector3 windVelocity)
    {
        Vector3 airflow = windVelocity;
        float effectiveArea = ComputeProjectedArea(airflow);
        Vector3 normalWorld = GlobalTransform.Basis * LocalNormal;
        return normalWorld * effectiveArea * LiftCoefficient * airflow.LengthSquared();
    }

    public Vector3 ComputeDrag(Vector3 windVelocity)
    {
        Vector3 airflow = windVelocity;
        float effectiveArea = ComputeProjectedArea(airflow);
        return -airflow.Normalized() * effectiveArea * DragCoefficient * airflow.LengthSquared();
    }
}