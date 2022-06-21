using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4
{
    public partial class Form1 : Form
    {
        const float PI = (float)Math.PI;
        Timer tmr = new Timer() { Interval = 50 };
        GraphicsPath sinusoid;
        //PointF pt;
        int n;
        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            tmr.Tick += tmr_Tick;
            sinusoid = new GraphicsPath();
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            if (++n >= sinusoid.PointCount) n = 0;
            pictureBox1.Refresh();
        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (sinusoid.PointCount == 0) return;
            PointF pt = sinusoid.PathPoints[n];
            e.Graphics.ScaleTransform(1, -1);
            e.Graphics.TranslateTransform(5, -pictureBox1.Height / 2);
            e.Graphics.DrawPath(Pens.Red, sinusoid);

            e.Graphics.FillEllipse(Brushes.Blue, RectangleF.FromLTRB(pt.X - 15, pt.Y - 15, pt.X + 15, pt.Y + 15));
        }

        void button1_Click(object sender, EventArgs e)
        {
            n = 0;
            CreateSinusoid(sinusoid, 10f * PI, pictureBox1.ClientSize);
            if (tmr.Enabled)
                tmr.Stop();
            else
                tmr.Start();
        }
        private void CreateSinusoid(GraphicsPath gp, float MaxX, Size size)
        {
            gp.Reset();
            PointF[] points = new PointF[1] { PointF.Empty };
            for (float i = 0; i <= MaxX; i += 0.2f)
            {
                points[points.GetUpperBound(0)] = new PointF(i, (float)Math.Sin(i));
                Array.Resize<PointF>(ref points, points.Length + 1);
            }
            Array.Resize<PointF>(ref points, points.Length - 1);
            gp.AddCurve(points);
            Matrix m = new Matrix();
            m.Scale((float)(size.Width / MaxX), 0.4f * size.Height);
            gp.Transform(m);
        }
    }
}
