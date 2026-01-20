
namespace FlightModel1.ControlState;

public record UserInputs(
    ActionRecord[] AxisInput,
    ActionRecord[] ActionInputs
);