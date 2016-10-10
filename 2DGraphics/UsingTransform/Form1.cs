using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GraphicsSandbox
{
    /// <summary>
    /// A form class used in the demo application
    /// </summary>
    public partial class Form1 : Form
    {

        Timer tmr = new Timer();
        GraphicsPath gp = new GraphicsPath();
        Pen pen = new Pen(Color.Red, 1);

        Point center { get; set; }

        IEnumerator<float> costable;


        public Form1()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);


            //Configure timer
            tmr.Interval = 5;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;

            //Init a graphics path and a cosinus table values for matrix rotation
            using (Font largeFont = new Font(Font.FontFamily, Font.Size * 12, FontStyle.Bold, GraphicsUnit.Pixel))
            {



                SizeF sz = this.CreateGraphics().MeasureString(this.Text, largeFont);

                var ptText = new Point(70, 70);
                gp.AddString(this.Text, largeFont.FontFamily, (int)largeFont.Style, largeFont.Size, ptText, StringFormat.GenericDefault);


                center = new Point((int)(ptText.X + sz.Width / 2), (int)(ptText.Y + sz.Height / 2));

                //Make an enumerator ready to deliver cos values
                costable = (new List<double>() { 
                    0, 10, 20, 30, 40, 50, 60, 70, 80, 90,  
                }).Select(v => (float)Math.Cos(v * Math.PI / 180)).ToList().GetEnumerator();

                //Sample - How to make an inline lambda function
                string temp = new Func<string>(() => "test")();


                //Another version of costable
                costable = new Func<List<float>>(() =>
                {
                    List<float> nn = new List<float>();
                    for (int v = 0; v < 360; v++)
                    {
                        nn.Add((float)Math.Cos(v * Math.PI / 180));
                    }

                    return nn;
                }

                    )().GetEnumerator();

            }

        }


        /// <summary>
        /// Typical way to prevent flickering in Winform applications
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        //Timer event
        void tmr_Tick(object sender, EventArgs e)
        {
            //Make label redraw
            label1.Invalidate();
        }


        private void label1_Paint(object sender, PaintEventArgs e)
        {

            //A bit silly access to an IEnumarable as it picks a new rotation
            //angle on each paint
            if (!costable.MoveNext())
            {
                costable.Reset();
                costable.MoveNext();
            };


            {

                var T = new Matrix();
                //Focus the matrix operations to center
                T.Translate(center.X, center.Y);

                //Do whatever needed arround the center
                //T.Rotate(1);
                T.Scale(costable.Current, 1f, MatrixOrder.Prepend);

                //Translate back to use matrix
                T.Translate(-center.X, -center.Y);

                //Apply transform
                e.Graphics.Transform = T;

                //Draw
                e.Graphics.DrawPath(pen, gp);

            }



        }



    }
}
