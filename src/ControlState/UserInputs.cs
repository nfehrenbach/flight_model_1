
namespace FlightModel1.ControlState;
// TODO: NEON-23 may need a refactor
public record UserInputs(
    ActionRecord[] AxisInput,
    ActionRecord[] ActionInputs
);