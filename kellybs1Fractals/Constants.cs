using System.Drawing;

/*
* Class: Constants
* Description: Contains constant values used by Fractals project
* Author: Brendan Kelly
* Date: 14/8/2017
*/

namespace kellybs1Fractals
{
    public static class Constants
    {
        //General
        public const int DISPLAY_PANEL_SIZE = 640;
        public const double DEGS_TO_RADS = 0.0174532925;
        public const double RADS_TO_DEGS = 57.295779513;

        //Suggested depths
        public const int SUGGESTED_DEPTH_TREE = 13;
        public const int SUGGESTED_DEPTH_KOCHFLAKE = 4;
        public const int SUGGESTED_DEPTH_SIERGASKET = 7;
        public const int SUGGESTED_DEPTH_VICSEK = 5;
        public const int SUGGESTED_DEPTH_ALT_VICSEK = 5;
        public const int SUGGESTED_DEPTH_DRAGON = 14;

        //Tree
        public const float TREE_PEN_START_WIDTH = 17f;
        public const float TREE_PEN_REDUCE = 1.8f;
        public const double TREE_START_ANGLE = -90; //90 = straight down -90 = straight up
        public const int TREE_BRANCH_ANGLE_MODIFIER_MIN = 10;
        public const int TREE_BRANCH_ANGLE_MODIFIER_MAX = 30;
        public const double TREE_BRANCH_LEN_MODIFIER = 4.6;

        //Tree branch colours
        public static Color DARK_BROWN = Color.FromArgb(89, 55, 10);
        public static Color MUD_BROWN = Color.FromArgb(114, 70, 13);
        public static Color LIGHT_BROWN = Color.FromArgb(150, 89, 10);
        public static Color LIGHTER_BROWN = Color.FromArgb(183, 115, 27);
        public static Color PEACH_BROWN = Color.FromArgb(198, 138, 89);
        public static Color LEAF_GREEN = Color.FromArgb(99, 140, 63);
        public const int BRANCH_SHADER_DIVISION_1 = 3;
        public const int BRANCH_SHADER_DIVISION_2 = 6;
        public const int BRANCH_SHADER_DIVISION_3 = 7;
        public const int BRANCH_SHADER_DIVISION_4 = 9;
        public const int BRANCH_SHADER_DIVISION_5 = 11;

        //Koch Snowflake
        public const int KOCH_DEGREE_ROTATE = 60;
        public const int KOCH_LINE_2_ROTATE = 120;
        public const int KOCH_LINE_3_ROTATE = 240;
        public const int KOCH_LINE_LEN_START = 500;
        public const int SPACING_TOP = 90;
        public const float KOCH_PEN = 2f;
        public const int COLOUR_GEN_MIN = 15;
        public const int COLOUR_GEN_MAX = 230;

        //Sierpinski Gasket
        public const int SIER_GASKET_STARTXY = 64;
        public const int SIER_GASKET_START_LEN = 512;
        public const float SIER_GASKET_PEN = 2f;

        //Dragon
        public const int DRAGON_ROTATE_DEG = 45;
        public const float DRAGON_START_X = 150;
        public const float DRAGON_START_Y = 256;
        public const float DRAGON_START_LENGTH = 384;
    }
}
