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
    /// No beauty - just som code here and there to get the basic idea how to apply a matrix
    /// to transform your graphics. Rotate, Translate etc
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



            tmr.Interval = 5;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;

            using (Font largeFont = new Font(Font.FontFamily, Font.Size * 12, FontStyle.Bold, GraphicsUnit.Pixel))
            {



                SizeF sz = this.CreateGraphics().MeasureString(this.Text, largeFont);

                var ptText = new Point(70, 70);
                gp.AddString(this.Text, largeFont.FontFamily, (int)largeFont.Style, largeFont.Size, ptText, StringFormat.GenericDefault);


                center = new Point((int)(ptText.X + sz.Width / 2), (int)(ptText.Y + sz.Height / 2));

                costable = (new List<double>() { 
                    0, 10, 20, 30, 40, 50, 60, 70, 80, 90,  
                }).Select(v => (float)Math.Cos(v * Math.PI / 180)).ToList().GetEnumerator();


                string temp = new Func<string>(() => "test")();



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
        /// What is this
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

        void tmr_Tick(object sender, EventArgs e)
        {
            label1.Invalidate();
        }


        private void label1_Paint(object sender, PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            //base.OnPaint(e);
            // Call methods of the System.Drawing.Graphics object.

            if (!costable.MoveNext())
            {
                costable.Reset();
                costable.MoveNext();
            };


            {

                var T = new Matrix();
                T.Translate(center.X, center.Y);
                //T.Rotate(1);
                T.Scale(costable.Current, 1f, MatrixOrder.Prepend);
                T.Translate(-center.X, -center.Y);
                e.Graphics.Transform = T;
                e.Graphics.DrawPath(pen, gp);

            }



        }



    }
}
