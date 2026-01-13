using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters;
using Godot;

namespace FlightModel1;

#nullable enable

public static class Util
{
    public static bool IsAlive<T>([NotNullWhen(true)] T? node) where T : Node
        => node != null && GodotObject.IsInstanceValid(node);
}