﻿using System;
using System.Drawing;

/*
 * Fractal.cs
 * Description: File contains classes, methods, and enum for all things Fractal
 * Author: Brendan Kelly
 * Date: 14/8/2017 - 22/8/2017
 */

namespace kellybs1Fractals
{
    //enum containing names of all implemented fractals
    public enum EFractalType { Tree, KochSnowflake, SierpinskiGasket, Vicsek, AltVicsek, Dragon }

    /*
     * Class: Fractal
     * Description: Base class for deriving Fractal implementations
     * Author: Brendan Kelly
     * Date: 14/8/2017 - 21/8/2017
     */
    public abstract class Fractal
    {
        protected Pen fracPen;
        protected Random rand;

        public Fractal()
        {
            rand = new Random();
            //initialise the pen, although it's generally changed before being used
            fracPen = new Pen(Color.Black);
            fracPen.Width = 1;
        }

        //randomly generates colours (but not all white or all black)
        protected Color RandColour()
        {
            int r = rand.Next(Constants.COLOUR_GEN_MIN, Constants.COLOUR_GEN_MAX);
            int g = rand.Next(Constants.COLOUR_GEN_MIN, Constants.COLOUR_GEN_MAX);
            int b = rand.Next(Constants.COLOUR_GEN_MIN, Constants.COLOUR_GEN_MAX);
            return Color.FromArgb(r, g, b);
        }

        //calculates a new point based on distance and length between old and new point
        //directly taken from Powerpoint example code on drawing at an angle
        protected virtual PointF GetNextPoint(double angle, double len, float pointX, float pointY)
        {
            PointF nextPoint = new PointF(0f, 0f);
            //calculate the next point from angle and distance
            double angleRads = angle * Constants.DEGS_TO_RADS;
            double xDelta = pointX + Math.Cos(angleRads) * len;
            double yDelta = pointY + Math.Sin(angleRads) * len;
            nextPoint.X = (float)xDelta;
            nextPoint.Y = (float)yDelta;
            return nextPoint;
        }

        //disables anti-aliasing (edge-smoothing)
        protected virtual void DisableAA(Graphics canvas)
        {
            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }

        //enables anti-aliasing (edge-smoothing)
        protected virtual void EnableAA(Graphics canvas)
        {
            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public abstract void Run(Graphics canvas, int depth);  
    }
    // END Abstract Fractal -------------------------------------------------------------------

    /*
     * Class: FractalFactory
     * Description: A simple factory class for generating Fractal object based on input
     * Author: Brendan Kelly
     * Date: 14/8/2017 - 19/7/2017
     */
    public class FractalFactory
    {
        //generate the chosen fractal object
        public Fractal GenerateFractal(EFractalType fractalType)
        {          
            Fractal newFractal = null;
            //choose fractal based on enum input
            switch (fractalType)
            {
                case EFractalType.Tree:
                    newFractal = new Tree();
                    break;

                case EFractalType.KochSnowflake:
                    newFractal = new KochSnowflake();
                    break;

                case EFractalType.SierpinskiGasket:
                    newFractal = new SierpinskiGasket();
                    break;

                case EFractalType.Vicsek:
                    newFractal = new Vicsek();
                    break;

                case EFractalType.AltVicsek:
                    newFractal = new AltVicsek();
                    break;

                case EFractalType.Dragon:
                    newFractal = new Dragon();
                    break;
            }
            //give it back
            return newFractal;
        }
    }
    // END FractalFactory ---------------------------------------------------------------------

    /*
     * Class: Tree
     * Description: An implementation of a simple Tree fractal
     * Author: Brendan Kelly
     * Date: 14/8/2017 - 17/8/2017
     */

    /*
     * Logical Description:
     * The Tree fractal automatically stops at depth 0. At depth one, a single line (the "trunk")
     * is drawn at the given start angle. 
     * While the depth is greater than one, a branch is drawn at a positive angle, starting 
     * at the end of the previous branch, the then fractal recurses with a reduced depth value. 
     * The same recursion is repeating for a second branch, with with a negative angle instead of a positive angle.
     * Additional Notes: This fractal is modified to reduce the length
     * of each branch and the thickness of the drawing line on-the-fly, based on decreasing variables.
     * The angles of new branches are generated by a random value chosen between a sensible range, to give the branching 
     * a more natural appearance.
     * The colour on the drawn line is also shaded by a very basic shading rainbow algorithm, based on the depth value.
     * I spent too much time making this tree pretty.
     */
    public class Tree : Fractal
    {
        //initialisation method - triggers recursive method with starting parameters
        public override void Run(Graphics canvas, int depth)
        {
            //start at halfway x / part-way y
            int startX = Constants.DISPLAY_PANEL_SIZE / 2;
            int startY = Constants.DISPLAY_PANEL_SIZE - Constants.DISPLAY_PANEL_SIZE / 6;                                  
            double startAngle = Constants.TREE_START_ANGLE; //if this isn't -90 things get weird
            //now go!
            recursiveTree(canvas, depth, startX, startY, startAngle, Constants.TREE_PEN_START_WIDTH);
        }

        //the recursive tree drawing method
        private void recursiveTree(Graphics canvas, int depth, float startX, float startY, double angle, float penSize)
        {
            if (depth >= 1) 
            {
                //smaller pen width
                fracPen.Width = penSize;
                //get branch colour based on depth
                fracPen.Color = shadeBranches(depth);               
                // branch length absed on depth (note - use the depth or pensize instead of another decreasing variable):
                double len = depth * Constants.TREE_BRANCH_LEN_MODIFIER;
                //calculate new x and y points based on previous angle and branch length
                PointF endPoint = GetNextPoint(angle, len, startX, startY);
                //find new x and y - 
                float endX = endPoint.X;
                float endY = endPoint.Y;
                //draw the line here - before new calculations
                canvas.DrawLine(fracPen, startX, startY, endX, endY);
                //generate new angles for the tree branches - left and right
                //as long as one branch is - the modifier and the other is + the modifier \/               
                int rightModifier = rand.Next(Constants.TREE_BRANCH_ANGLE_MODIFIER_MIN, Constants.TREE_BRANCH_ANGLE_MODIFIER_MAX);
                //recurse with reduced depth and pen width
                recursiveTree(canvas, depth - 1, endX, endY, angle + rightModifier, penSize - Constants.TREE_PEN_REDUCE);
                //same for other branch
                int leftModifier = rand.Next(Constants.TREE_BRANCH_ANGLE_MODIFIER_MIN, Constants.TREE_BRANCH_ANGLE_MODIFIER_MAX);
                recursiveTree(canvas, depth - 1, endX, endY,  angle - leftModifier,  penSize - Constants.TREE_PEN_REDUCE);
            }             
        }

        //provides different colours for branches - a kind of lazy rainbow shader
        private Color shadeBranches(int depth)
        {
            if (depth < Constants.BRANCH_SHADER_DIVISION_1)
                return Constants.LEAF_GREEN;
            else if (depth < Constants.BRANCH_SHADER_DIVISION_2)
                return Constants.PEACH_BROWN;
            else if (depth < Constants.BRANCH_SHADER_DIVISION_3)
                return Constants.LIGHTER_BROWN;
            else if (depth < Constants.BRANCH_SHADER_DIVISION_4)
                return Constants.LIGHT_BROWN;
            else if (depth < Constants.BRANCH_SHADER_DIVISION_5)
                return Constants.MUD_BROWN;
            else
                return Constants.DARK_BROWN;
        }
    }
    // END Tree -------------------------------------------------------------------------------

    /*
    * Class: KochSnowflake
    * Description: A recursive implementation of Koch's Snowflake fractal
    * Author: Brendan Kelly
    * Date: 15/8/2017 - 17/8/2017
    */

    /*
     * Logical Description:
     * The Koch Curve at zero depth draws a straight line. If the depth is higher than zero, the line is broken 
     * into three sections, and the middle section replaced with two sides of an equilateral triangle to form a
     * new line with an appearance similar to _/\_ . In recursion this line breaking algorithm is subsequently 
     * applied to each straight section of the line, until the depth value equals zero.
     * Additional Notes: The Koch Snowflake performs this algorithm on three Koch Curves arranged in a triangle.
     * Each individual line is drawn with a randomised colour.
     */
    public class KochSnowflake : Fractal
    {
        public KochSnowflake()
        {
            fracPen.Width = Constants.KOCH_PEN;
        }
        public override void Run(Graphics canvas, int depth)
        {
            //find a starting point
            float startX = Constants.DISPLAY_PANEL_SIZE / 8;
            float startY = Constants.DISPLAY_PANEL_SIZE / 8 + Constants.SPACING_TOP;
            double startLen = Constants.KOCH_LINE_LEN_START;
            double startAngle = 0;
            //work out second line start
            PointF point2 = GetNextPoint(startAngle, startLen, startX, startY);
            float l2StartX = point2.X;
            float l2StartY = point2.Y;
            //work out 3rd line start
            PointF point3 = GetNextPoint(startAngle + Constants.KOCH_LINE_2_ROTATE, startLen, l2StartX, l2StartY);
            float l3StartX = point3.X;
            float l3StartY = point3.Y;
            //draw three Koch Curves in a triangle
            recursiveKoch(canvas, depth, startX, startY, startAngle, startLen); //zero degrees start angle     
            recursiveKoch(canvas, depth, l2StartX, l2StartY, startAngle + Constants.KOCH_LINE_2_ROTATE, startLen);
            recursiveKoch(canvas, depth, l3StartX, l3StartY, startAngle + Constants.KOCH_LINE_3_ROTATE, startLen);
        }

        //the Koch Curve recursive method
        private void recursiveKoch(Graphics canvas, int depth, float startX, float startY, double angle, double length)
        {
            if (depth == 0) //base case - time to draw!
            {
                //find the endpoint of the current line
                PointF endPoint = GetNextPoint(angle, length, startX, startY);
                float endX = endPoint.X;
                float endY = endPoint.Y;
                //generate new color and draw the line
                fracPen.Color = RandColour();
                canvas.DrawLine(fracPen, startX, startY, endX , endY);
            }
            else
            {
                double angleRads = angle * Constants.DEGS_TO_RADS;
                //points 1-5 are the startpoints and corners on one Koch curve, from left to right 1__2/3\4__5
                //point 5 (endY is calculated at draw)
                float pt1X = startX;
                float pt1Y = startY;
                //find point 2 - it's 1/3 of the way along the line - trigonometry is required
                double lineDivLen = length / 3;
                double tempXDelta = Math.Cos(angleRads) * lineDivLen;
                double tempYDelta = Math.Sin(angleRads) * lineDivLen;
                float pt2X = (float)(pt1X + tempXDelta);
                float pt2Y = (float)(pt1Y + tempYDelta);
                //find point 3 - because we're making an equilateral triangle here 
                //we know we're going 1/3 line length at 60 degrees
                double sectionRotation = Constants.KOCH_DEGREE_ROTATE * Constants.DEGS_TO_RADS;
                tempXDelta = Math.Cos(angleRads - sectionRotation) * lineDivLen;
                tempYDelta = Math.Sin(angleRads - sectionRotation) * lineDivLen;
                float pt3X = (float)(pt2X + tempXDelta);
                float pt3Y = (float)(pt2Y + tempYDelta);
                //find point 4 - again going 1/3 line length at 60 degrees in the other direction
                tempXDelta = Math.Cos(angleRads + sectionRotation) * lineDivLen;
                tempYDelta = Math.Sin(angleRads + sectionRotation) * lineDivLen;
                float pt4X = (float)(pt3X + tempXDelta);
                float pt4Y = (float)(pt3Y + tempYDelta);
                //recursive calls - rotate 2 and 3: / \
                recursiveKoch(canvas, depth - 1, pt1X, pt1Y, angle, lineDivLen);
                recursiveKoch(canvas, depth - 1, pt2X, pt2Y, angle - Constants.KOCH_DEGREE_ROTATE, lineDivLen);
                recursiveKoch(canvas, depth - 1, pt3X, pt3Y, angle + Constants.KOCH_DEGREE_ROTATE, lineDivLen);
                recursiveKoch(canvas, depth - 1, pt4X, pt4Y, angle, lineDivLen);
            }
        }
    }
    // END KochSnowflake -------------------------------------------------------------------------------

    /*
    * Class: SierpinskiGasket
    * Description: A recursive implementation of a Sierpinski Gasket fractal
    * Author: Brendan Kelly
    * Date: 16/8/2017 - 17/8/2017
    */

    /*
    * Logical Description:
    * The Sierpinski Gasket at zero depth draws a single square. At higher depth the current square is divided into
    * four equal squares and the top left, top right, and bottom right squares are used in three separate recursions
    * until the depth equals zero.
    */

    public class SierpinskiGasket : Fractal
    {
        Brush fill;     
        public SierpinskiGasket()
        {
            fill = new SolidBrush(Color.MediumSeaGreen);
            fracPen.Width = Constants.SIER_GASKET_PEN;
        }
        public override void Run(Graphics canvas, int depth)
        {
            //disable antialiasing for gasket - the smoothing messes up the filled rectangles edges
            DisableAA(canvas);
            //initialise start points
            float startX = Constants.SIER_GASKET_STARTXY;
            float startY = Constants.SIER_GASKET_STARTXY;
            double startLen = Constants.SIER_GASKET_START_LEN;
            //Go!
            recursiveSierGasket(canvas, depth, startX, startY, startLen);
            //draw outline around Gasket when done for context
            canvas.DrawRectangle(fracPen, startX, startY, (float)startLen, (float)startLen);
            //turn antialising back on for other fractals
            EnableAA(canvas);
        }

        //Draws a recursive Sierpinkski's Gasket
        private void recursiveSierGasket(Graphics canvas, int depth, float startX, float startY, double length)
        {         
            if (depth == 0) //base case - draw!
            {
                //fill the rectangle               
                canvas.FillRectangle(fill, startX, startY, (float)length, (float)length);
                //outline because it's pretty
                canvas.DrawRectangle(fracPen, startX, startY, (float)length, (float)length);
            }
            else
            {
                //calculate new points - divide current square into 4 squares
                double halfWay = length / 2;
                float upperLeftX = startX;
                float upperLeftY = startY;
                float upperRightX = (float)(startX + halfWay);
                float upperRightY = startY;
                float lowerRightX = (float)(startX + halfWay);
                float lowerRightY = (float)(startY + halfWay);
                //recurse 3x - don't recurse into lower left
                recursiveSierGasket(canvas, depth - 1, upperRightX, upperRightY, halfWay);
                recursiveSierGasket(canvas, depth - 1, lowerRightX, lowerRightY, halfWay);
                recursiveSierGasket(canvas, depth - 1, upperLeftX, upperLeftY, halfWay);
            }
        }      
    }
    // END Sierpinkski Gasket -------------------------------------------------------------------------------

  /*
   * Class: Vicsek
   * Description: A recursive implementation of a Vicsek or Box fractal
   * Author: Brendan Kelly
   * Date: 17/8/2017
   */

    /*
     * Logical Description:
     * The Vicsek Fractal draws a single square at zero depth. At higher depths, the square is divided into
     * a 3x3 grid, and recurse in the five inner squares, leaving out the corner squares.
     * The alternate version draws a circle first, then divides the area into a 3x3 grid and recurses in the same manner.
     */
    public class Vicsek : Fractal
    {
        Brush fill;
        public Vicsek()
        {
            fill = new SolidBrush(Color.OrangeRed);
        }
        public override void Run(Graphics inCanvas, int depth)
        {
            //disable antialiasing for gasket - the smoothing messes up the filled rectangles edges
            DisableAA(inCanvas);

            //Go - full size
            recursiveVicsek(inCanvas, depth, 0, 0, Constants.DISPLAY_PANEL_SIZE - 1);

            //turn antialising back on for other fractals
            EnableAA(inCanvas);
        }

        //recursive method that draws Vicsek Fractal
        private void recursiveVicsek(Graphics canvas, int depth, float startX, float startY, double length)
        {
            if (depth == 0) //base case - draw!
            {
                //fill the shape          
                canvas.FillRectangle(fill, startX, startY, (float)length, (float)length);
                //outline 
                canvas.DrawRectangle(fracPen, startX, startY, (float)length, (float)length);
            }
            else
            {
                //break the area into 3x3 grid - but only calculate cross shape + 
                double thirdWay = length / 3;
                float upperX = (float)(startX + thirdWay);
                float upperY = startY;
                float leftX = startX;
                float leftY = (float)(startY + thirdWay);
                float midX = upperX;
                float midY = leftY;
                float rightX = (float)(midX + thirdWay);
                float rightY = leftY;
                float bottomX = upperX;
                float bottomY = (float)(leftY + thirdWay);
                //recurse each section
                recursiveVicsek(canvas, depth - 1, upperX, upperY, thirdWay);
                recursiveVicsek(canvas, depth - 1, leftX, leftY, thirdWay);
                recursiveVicsek(canvas, depth - 1, midX, midY, thirdWay);
                recursiveVicsek(canvas, depth - 1, rightX, rightY, thirdWay);
                recursiveVicsek(canvas, depth - 1, bottomX, bottomY, thirdWay);
            }
        }
    }

  /*
   * Class: AltVicsek
   * Description: A alternate recursive implementation of a Vicsek or Box fractal with the drawing performed each recursive step
   * Author: Brendan Kelly
   * Date: 17/8/2017
   */
    public class AltVicsek : Fractal
    {
        public override void Run(Graphics canvas, int depth)
        {
            //Go - full size
            recursiveVicsek(canvas, depth, 0, 0, Constants.DISPLAY_PANEL_SIZE - 1);
        }

        //recursive method that draws Vicsek Fractal
        private void recursiveVicsek(Graphics canvas, int depth, float startX, float startY, double length)
        {
            if (depth >= 0) 
            {
                //draw
                canvas.DrawEllipse(fracPen, startX, startY, (float)length, (float)length);

                //break the area into 3x3 grid - but only calculate cross shape + 
                double thirdWay = length / 3;
                float upperX = (float)(startX + thirdWay);
                float upperY = startY;
                float leftX = startX;
                float leftY = (float)(startY + thirdWay);
                float midX = upperX;
                float midY = leftY;
                float rightX = (float)(midX + thirdWay);
                float rightY = leftY;
                float bottomX = upperX;
                float bottomY = (float)(leftY + thirdWay);

                //recurse each section
                recursiveVicsek(canvas, depth - 1, upperX, upperY, thirdWay);
                recursiveVicsek(canvas, depth - 1, leftX, leftY, thirdWay);
                recursiveVicsek(canvas, depth - 1, midX, midY, thirdWay);
                recursiveVicsek(canvas, depth - 1, rightX, rightY, thirdWay);
                recursiveVicsek(canvas, depth - 1, bottomX, bottomY, thirdWay);
            }
        }
    }

    // END Vicsek -------------------------------------------------------------------------------

    /*
    * Class: Dragon
    * Description: A recursive implementation of a Dragon curve
    * Author: Brendan Kelly
    * Date: 19/8/2017
    */

    /*
    * Logical Description:
    * The Dragon Curve draws a straight line at depth zero. At higher depth, a line is divided
    * into two lines by forming an isoceles right triangle with a hypotenuse the length of the original
    * line. The two new lines formed by this calculation are then recursed into again with reduced 
    * depth and length.
    */
    public class Dragon : Fractal
    {
        public Dragon()
        {
            fracPen.Color = Color.Indigo;         
        }
        public override void Run(Graphics inCanvas, int depth)
        {
            //initialise values
            float startX = Constants.DRAGON_START_X;
            float startY = Constants.DRAGON_START_Y;
            float startLen = Constants.DRAGON_START_LENGTH;
            //now go
            recursiveDragon(inCanvas, depth, startX, startY, startX + startLen, startY, startLen);

        }

        //recursively draws a Dragon curve
        public void recursiveDragon(Graphics canvas, int depth, float startX, float startY, float endX, float endY, double length)
        {
            if (depth == 0) // base case - draw line
                canvas.DrawLine(fracPen, startX, startY, endX, endY);
            else
            {
                //find new side length via pythagoras
                double newLen = Math.Sqrt(length * length / 2);
                //find the new "corner" point
                //1 - find the angle between the current line's start and end points
                double angleBetween = Math.Atan2(endY - startY, endX - startX) * Constants.RADS_TO_DEGS;
                //2 - find the corner of the triangle by adding that angle to the current angle at current length
                PointF newCorner = GetNextPoint(angleBetween + Constants.DRAGON_ROTATE_DEG, newLen, startX, startY);
                //recurse both lines
                recursiveDragon(canvas, depth - 1, startX, startY, newCorner.X, newCorner.Y, newLen);
                recursiveDragon(canvas, depth - 1, endX, endY, newCorner.X, newCorner.Y,  newLen);
            }
        }    
    }
}