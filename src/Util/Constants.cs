
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
        public const string RootStr = "fuselage";
        public const string LeftWingStr
            = $"{RootStr}/receiver_wing_mount_l/socket_wing_mount_l/wing_pivot_mount_l/hinge_wing_l";
        public const string RightWingStr
            = $"{RootStr}/receiver_wing_mount_r/socket_wing_mount_r/wing_pivot_mount_r/hinge_wing_r";
        public const string LeftWingSurfStr = $"{LeftWingStr}/wing_l";
        public const string RightWingSurfStr = $"{RightWingStr}/wing_r";
        public const string LeftWingAileronStr = $"{LeftWingStr}/hinge_aileron_l";
        public const string RightWingAileronStr = $"{RightWingStr}/hinge_aileron_r";
        public const string LeftFlapsStr = $"{LeftWingSurfStr}/hinge_flaps_l";
        public const string RightFlapsStr = $"{RightWingSurfStr}/hinge_flaps_r";
        public const string LeftAileronStr = $"{LeftWingSurfStr}/hinge_aileron_l";
        public const string RightAileronStr = $"{RightWingSurfStr}/hinge_aileron_r";
        public const string LeftInnerSpoilerStr = $"{LeftWingSurfStr}/hinge_spoiler_inner_l";
        public const string RightInnerSpoilerStr = $"{RightWingSurfStr}/hinge_spoiler_inner_r";
        public const string LeftOuterSpoilerStr = $"{LeftWingSurfStr}/hinge_spoiler_outer_l";
        public const string RightOuterSpoilerStr = $"{RightWingSurfStr}/hinge_spoiler_outer_r";
        public const string LeftMiddleSpoilerStr = $"{LeftWingSurfStr}/hinge_spoiler_middle_l";
        public const string RightMiddleSpoilerStr = $"{RightWingSurfStr}/hinge_spoiler_middle_r";
        public const string LeftStabilatorStr
            = $"{RootStr}/receiver_stabilator_strut_l/socket_stabilator_strut_l/stabilator_strut_l/hinge_stabilator_l";
        public const string RightStabilatorStr
            = $"{RootStr}/receiver_stabilator_strut_r/socket_stabilator_strut_r/stabilator_strut_r/hinge_stabilator_r";
        public const string LeftElevatorStr
            = $"{RootStr}/receiver_elevator_strut_l/socket_elevator_strut_l/elevator_strut_l/hinge_elevator_l";
        public const string RightElevatorStr
            = $"{RootStr}/receiver_elevator_strut_r/socket_elevator_strut_r/elevator_strut_r/hinge_elevator_r";
        public const string LeftRudderStr = $"{RootStr}/receiver_vstab_l/socket_vstab_l/vstab_l/hinge_rudder_l";
        public const string RightRudderStr = $"{RootStr}/receiver_vstab_r/socket_vstab_r/vstab_r/hinge_rudder_r";
        public const string RudderStr = $"{RootStr}/receiver_vstab/socket_vstab/vstab/hinge_rudder";
        public const string ElevatorStr
            = $"{RootStr}/receiver_elevator_strut/socket_elevator_strut/elevator_strut/hinge_elevator";
        public const string StabilatorStr
            = $"{RootStr}/receiver_stabilator_strut/socket_stabilator_strut/stabilator_strut/hinge_stabilator";
    }

    public static class ButtonNames
    {
        public const string LayerL = "LayerL";
        public const string LayerR = "LayerR";
    }
}
