/*
 * This control is an Angle select Control (photoshop style).
 * Enjoy using it.
 * Greetings, Roey (Israel).
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AngleSelect
{
    public delegate void AngleChanged(int Angle);

    public partial class AngleSelect : UserControl
    {
        private int angle = 0;
        private Rectangle ClientRect;
        private SolidBrush FillBrush = new SolidBrush(SystemColors.Control);
        private Pen LinePen = new Pen(Color.Black, 2.0f);
        private bool Clicked = false;
        public event AngleChanged angleChanged;

        public AngleSelect()
        {
            InitializeComponent();
            ClientRect = ClientRectangle;
            ClientRect.Width--;
            ClientRect.Height--;
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(FillBrush, ClientRect);
            e.Graphics.DrawPie(Pens.Black, ClientRect, angle, 360.0f);
            e.Graphics.DrawEllipse(LinePen, ClientRect);
        }
        /// <summary>
        /// Event - Raised control size is changed.
        /// </summary>
        private void AngleSelect_Resize(object sender, EventArgs e)
        {
            ClientRect = ClientRectangle;
            ClientRect.Width--;
            ClientRect.Height--;
            //ClientRect.Inflate(-20, -20);
        }
        /// <summary>
        /// Gets or Sets the angle position.
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                Refresh();
            }
        }
        /// <summary>
        /// Sets or Gets the Color or the surrounding line.
        /// </summary>
        public Color LineColor
        {
            get { return LinePen.Color; }
            set 
            { 
                LinePen.Color = value;
                Refresh();
            }
        }
        /// <summary>
        /// Sets or Gets the Fill Color of the circle.
        /// </summary>
        public Color FillColor
        {
            get { return FillBrush.Color; }
            set 
            { 
                FillBrush.Color = value;
                Refresh();
            }
        }
        /// <summary>
        /// Event - User has clicked the mouse, changing Angle
        /// </summary>
        private void AngleSelect_MouseDown(object sender, MouseEventArgs e)
        {
            Clicked = true;
            Point centerPoint = new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
            Point mousePos = ((MouseEventArgs)e).Location;
            double radians = Math.Atan2(mousePos.Y - centerPoint.Y, mousePos.X - centerPoint.X);
            angle = (int)(radians * (180 / Math.PI));
            Refresh();
            //call delegated function
            try
            {
                angleChanged(angle);
            }
            catch { }
        }
        /// <summary>
        /// Event - User has left the mouse button, stop change the Angle.
        /// </summary>
        private void AngleSelect_MouseUp(object sender, MouseEventArgs e)
        {
            Clicked = false;
        }
        /// <summary>
        /// Event - User is moving the mouse.
        /// </summary>
        private void AngleSelect_MouseMove(object sender, MouseEventArgs e)
        {
            if (Clicked == true)
            {
                //if mouse down, change the Angle.
                Point centerPoint = new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
                Point mousePos = ((MouseEventArgs)e).Location;
		        //Using the Atan2 function in order to get the Angle of the Slope between the center Point of the control and the Mouse Point.
                double radians = Math.Atan2(mousePos.Y - centerPoint.Y, mousePos.X - centerPoint.X);
		        //Then converting from Radians to regular Units.
                angle = (int)(radians * (180 / Math.PI));
                Refresh();
                //call delegated function
                try
                {
                    angleChanged(angle);
                }
                catch { }
            }
        }
    }
}