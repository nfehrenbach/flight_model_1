
namespace FlightModel1.Utils;

public static class Consts
{
    public static class AxisNames
    {
        /// <summary>
        /// Typically maps to LStick-X+
        /// </summary>
        public const string Axis1Up = "Axis1Up";

        /// <summary>
        /// Typically maps to LStick-X-
        /// </summary>
        public const string Axis1Down = "Axis1Down";

        /// <summary>
        /// Typically maps to LStick-Y+
        /// </summary>
        public const string Axis2Up = "Axis2Up";

        /// <summary>
        /// Typically maps to LStick-Y-
        /// </summary>
        public const string Axis2Down = "Axis2Down";

        /// <summary>
        /// Typically maps to RTrigger
        /// </summary>
        public const string Axis3Up = "Axis3Up";

        /// <summary>
        /// Typically maps to LTrigger
        /// </summary>
        public const string Axis3Down = "Axis3Down";

        /// <summary>
        /// Typically maps to RStick-X+
        /// </summary>
        public const string Axis4Up = "Axis4Up";

        /// <summary>
        /// Typically maps to RStick-X-
        /// </summary>
        public const string Axis4Down = "Axis4Down";

        /// <summary>
        /// Typically maps to RStick-Y+
        /// </summary>
        public const string Axis5Up = "Axis5Up";

        /// <summary>
        /// Typically maps to RStick-Y-
        /// </summary>
        public const string Axis5Down = "Axis5Down";

        /// <summary>
        /// Typically maps to RBumper or similar
        /// </summary>
        public const string Axis6Up = "Axis6Up";

        /// <summary>
        /// Typically maps to LBumper or similar
        /// </summary>
        public const string Axis6Down = "Axis6Down";
    }

    public static class PlaneGeoAddr
    {
        public const string rootStr = "fuselage";
        public const string leftWingStr = $"{rootStr}/receiver_wing_mount_l/socket_wing_mount_l/wing_pivot_mount_l/hinge_wing_l";
        public const string rightWingStr = $"{rootStr}/receiver_wing_mount_r/socket_wing_mount_r/wing_pivot_mount_r/hinge_wing_r";
        public const string leftWingSurfStr = $"{leftWingStr}/wing_l";
        public const string rightWingSurfStr = $"{rightWingStr}/wing_r";
        public const string leftFlapsStr = $"{leftWingSurfStr}/hinge_flaps_l";
        public const string rightFlapsStr = $"{rightWingSurfStr}/hinge_flaps_r";
        public const string leftAileronStr = $"{leftWingSurfStr}/hinge_aileron_l";
        public const string rightAileronStr = $"{rightWingSurfStr}/hinge_aileron_r";
        public const string leftInnerSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_inner_l";
        public const string rightInnerSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_inner_r";
        public const string leftOuterSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_outer_l";
        public const string rightOuterSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_outer_r";
        public const string leftMiddleSpoilerStr = $"{leftWingSurfStr}/hinge_spoiler_middle_l";
        public const string rightMiddleSpoilerStr = $"{rightWingSurfStr}/hinge_spoiler_middle_r";
        public const string leftStabilatorStr = $"{rootStr}/receiver_stabilator_strut_l/socket_stabilator_strut_l/stabilator_strut_l/hinge_stabilator_l";
        public const string rightStabilatorStr = $"{rootStr}/receiver_stabilator_strut_r/socket_stabilator_strut_r/stabilator_strut_r/hinge_stabilator_r";
        public const string leftElevatorStr = $"{rootStr}/receiver_elevator_strut_l/socket_elevator_strut_l/elevator_strut_l/hinge_elevator_l";
        public const string rightElevatorStr = $"{rootStr}/receiver_elevator_strut_r/socket_elevator_strut_r/elevator_strut_r/hinge_elevator_r";
        public const string leftRudderStr = $"{rootStr}/receiver_vstab_l/socket_vstab_l/vstab_l/hinge_rudder_l";
        public const string rightRudderStr = $"{rootStr}/receiver_vstab_r/socket_vstab_r/vstab_r/hinge_rudder_r";
        public const string rudderStr = $"{rootStr}/receiver_vstab/socket_vstab/vstab/hinge_rudder";
    }

    public static class ButtonNames
    {
        public const string LayerL = "LayerL";
        public const string LayerR = "LayerR";
    }
}
