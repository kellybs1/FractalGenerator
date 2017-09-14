using System;
using System.Drawing;
using System.Windows.Forms;

namespace kellybs1Fractals
{
    public partial class Form1 : Form
    {
        private Graphics mainCanvas;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //force panel size correct - design is 512x512
            panelDisplay.Width = Constants.DISPLAY_PANEL_SIZE;
            panelDisplay.Height = Constants.DISPLAY_PANEL_SIZE;
            //pull combobox values from enum
            comboBoxFractal.DataSource = Enum.GetValues(typeof(EFractalType));
            //load the first suggested depth into depth selector
            loadSuggestedDepth();
            //make graphics
            mainCanvas = panelDisplay.CreateGraphics();
            //enable antialiasing = smoothes edges
            mainCanvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }


        //the Go button
        private void buttonGo_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            clearCanvas();
            //get values from controls
            EFractalType chosenFractal = (EFractalType)comboBoxFractal.SelectedItem;
            int depth = (int)numericUpDownDepth.Value;
            //create controller with form values
            FractalDisplayController fracController = new FractalDisplayController(mainCanvas, chosenFractal);
            fracController.RunFractal(depth);
            this.Cursor = Cursors.Default;
        }

        //clears the canvas by filling with white
        private void clearCanvas()
        {
            Brush b = new SolidBrush(Color.White);
            Rectangle fillRect = new Rectangle(0, 0, Constants.DISPLAY_PANEL_SIZE, Constants.DISPLAY_PANEL_SIZE);
            mainCanvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            mainCanvas.FillRectangle(b, fillRect);
            mainCanvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        //when the comobox selected item changes
        private void comboBoxFractal_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSuggestedDepth();
        }

        //update the suggest depth based on current chose fractal
        private void loadSuggestedDepth()
        {
            EFractalType currentSelectedFractal = (EFractalType)comboBoxFractal.SelectedItem;
            int suggestedDepth = 0;
            //pick the corresponding value to the selected item in the combo box
            switch (currentSelectedFractal)
            {
                case EFractalType.Tree:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_TREE;
                    break;

                case EFractalType.KochSnowflake:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_KOCHFLAKE;
                    break;

                case EFractalType.SierpinskiGasket:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_SIERGASKET;
                    break;

                case EFractalType.Vicsek:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_VICSEK;
                    break;

                case EFractalType.AltVicsek:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_ALT_VICSEK;
                    break;

                case EFractalType.Dragon:
                    suggestedDepth = Constants.SUGGESTED_DEPTH_DRAGON;
                    break;
            }
            //change the depth value automatically
            numericUpDownDepth.Value = suggestedDepth;
        }
     
    }
}
