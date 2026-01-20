using FlightModel1.Enum;
using Godot;

namespace FlightModel1.ControlState;

public class ActionRecord
{
    public string InputName { get; set; }
    public ActionState LastAction { get; set; }
    public long? LastActionTimeMs { get; private set; } = null;
    public float? Value { get; set; }

    public ActionRecord(string inputName, ActionState lastAction, float? value = null)
    {
        InputName = inputName;
        LastAction = lastAction;
        Value = value;
    }

    public void SetActionTime(long timeMs)
    {
        if (LastActionTimeMs != null)
            GD.PrintErr("Overwriting existing action time!");

        LastActionTimeMs = timeMs;
    }
}