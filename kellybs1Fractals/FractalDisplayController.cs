using System;
using System.Drawing;

/*
* Class: FractalDisplayManager
* Description: Class for managing interaction between form and Fractal drawing
* Author: Brendan Kelly
* Date: 14/8/2017
*/

namespace kellybs1Fractals
{
    public class FractalDisplayController
    {
        private FractalFactory fracFac;
        private Fractal currentFractal;
        private Graphics mainCanvas;
        public FractalDisplayController(Graphics inMainCanvas, EFractalType chosenFractal)
        {
            mainCanvas = inMainCanvas;
            fracFac = new FractalFactory();
            currentFractal = fracFac.GenerateFractal(chosenFractal);
        }

        //run the chosen fractal's generation algorith
        public void RunFractal(int depth)
        {
            currentFractal.Run(mainCanvas, depth);
        }
    }
}
