using Godot;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using FlightModel1.Enum;

namespace FlightModel1.ControlState;

#nullable enable
public class ActionHistory
{
    public ConcurrentDictionary<string, ActionRecord> Actions { get; private set; } = new();
    private Stopwatch stopwatch;

    public ActionHistory()
    {
        stopwatch = Stopwatch.StartNew();
    }

    public void AddActions(Inputs inputs)
    {
        long time = stopwatch.ElapsedMilliseconds;
        foreach (ActionRecord action in inputs.GetAllInputs())
        {
            action.SetActionTime(time);
            Actions.TryAdd(action.InputName, action);
        }
    }

    public bool ActionActiveAndWithin(string inputName, long deltaMs)
    {
        if (Actions.TryGetValue(inputName, out ActionRecord? record))
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            return (record.LastAction == ActionState.Entered || record.LastAction == ActionState.Held) &&
                   (currentTime - record.LastActionTimeMs <= deltaMs);
        }

        return false;
    }

    public bool ActionReleasedWithin(string inputName, long deltaMs)
    {
        if (Actions.TryGetValue(inputName, out ActionRecord? record))
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            return record.LastAction == ActionState.Released &&
                   (currentTime - record.LastActionTimeMs <= deltaMs);
        }

        return false;
    }
}