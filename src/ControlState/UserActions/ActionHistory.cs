using Godot;
using System.Collections.Concurrent;
using System.Diagnostics;
using FlightModel1.Enum;

namespace FlightModel1.ControlState.UserActions;

#nullable enable
public class ActionHistory
{
    public ConcurrentDictionary<string, ActionRecord> Actions { get; private set; } = new();
    private Stopwatch stopwatch;

    public ActionHistory()
    {
        stopwatch = Stopwatch.StartNew();
    }

    public void RecordAction(string inputName, ActionState state, float? value = 0f)
    {
        long currentTime = stopwatch.ElapsedMilliseconds;
        var record = new ActionRecord(inputName, state, currentTime, value);
        Actions.AddOrUpdate(inputName, record, (key, oldValue) => record);
    }

    public bool ActionActiveAndWithin(string inputName, long deltaMs)
    {
        if (Actions.TryGetValue(inputName, out ActionRecord? record))
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            return (record.State == ActionState.Entered || record.State == ActionState.Held) &&
                   (currentTime - record.LastActionTimeMs <= deltaMs);
        }

        return false;
    }

    public bool ActionReleasedWithin(string inputName, long deltaMs)
    {
        if (Actions.TryGetValue(inputName, out ActionRecord? record))
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            return record.State == ActionState.Released &&
                   (currentTime - record.LastActionTimeMs <= deltaMs);
        }

        return false;
    }
}