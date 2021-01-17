using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace _3SemLR5
{
    public partial class Form1 : Form
    {
        func[] f = new func[3];
        int Z = 5;
        double[][,] arr = new double[3][,];
        int z0;
        int N = 5; //must be odd
        double a = 0, b = 0, c = 0; //alpha, beta, gamma
        bool onmove = false;
        Point startpos;
        Color[] cl = {Color.LightBlue};
        delegate double func(double x, double y);

        public Form1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;            

            f[0] = f1;

            this.MouseWheel += new MouseEventHandler(this_MouseWheel);

            update();
            init();
        }

        double f1(double x, double y)
        {            
            return x*x+y*y/36-1;
        }

        void update()
        {
            chart1.ChartAreas[0].AxisX.Minimum = -N / 2;
            chart1.ChartAreas[0].AxisY.Minimum = -N / 2;
            chart1.ChartAreas[0].AxisX.Maximum = N / 2;
            chart1.ChartAreas[0].AxisY.Maximum = N / 2;
        }
        void init()
        {
            arr[0] = new double[N, N];
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                    arr[0][x, y] = f[0](x - N / 2, y - N / 2);
            }
            

            z0 = 2 * N * 3 + 3;

            for (int i = 0; i < 2 * N ; i++)
            {
                chart1.Series.Add("s" + i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
                chart1.Series[i].BorderWidth = trackBar1.Value;
            }

            for (int i = 0; i < 2 * N; i++)
            {
                chart1.Series[i].Color = colorDialog1.Color;
                chart1.Series[i].BorderWidth = trackBar1.Value;
            }

                for (int i = 2 * N; i < 2 * N + 3; i++)
            {
                chart1.Series.Add("a" + i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
                chart1.Series[i].Color = Color.Black;
            }
   
            for(int i = 2 * N + 3; i < 4 * N +3; i++)
            {
                chart1.Series.Add("s" + i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
                chart1.Series[i].Color = colorDialog1.Color;
                chart1.Series[i].BorderWidth = trackBar1.Value;
            }  
            
            drawscene();
        }

        void drawscene()
        {
            clear();
            drawxyz();
            draw(0);
        }

        void draw(int tn)
        {
            double[,] a = arr[tn];
            int n = tn * 2 * N;

            double X, Y, X1, Y1;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    X = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * a[x, y];
                    Y = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * a[x, y];
                    chart1.Series[n].Points.AddXY(X, Y);

                    if (checkBox1.Checked)
                    {
                        chart1.Series[n].Points[chart1.Series[n].Points.Count - 1].Label =
                                (x - N / 2).ToString() + ";" + (y - N / 2).ToString() + ";" + a[x, y].ToString("0.00");
                    }
                }
                n++;
            }

            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    X = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * a[x, y];
                    Y = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * a[x, y];
                    chart1.Series[n].Points.AddXY(X, Y);
                }
                n++;
            }
            n = n + 3;

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    X1 = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * (-a[x, y]);
                    Y1 = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * (-a[x, y]);
                    chart1.Series[n].Points.AddXY(X1, Y1);

                    if (checkBox1.Checked)
                    {
                        chart1.Series[n].Points[chart1.Series[n].Points.Count - 1].Label =
                                (x - N / 2).ToString() + ";" + (y - N / 2).ToString() + ";" + a[x, y].ToString("0.00");
                    }
                }
                n++;
            }

            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    X1 = l1() * (x - N / 2) + l2() * (y - N / 2) + l3() * (-a[x, y]);
                    Y1 = m1() * (x - N / 2) + m2() * (y - N / 2) + m3() * (-a[x, y]);
                    chart1.Series[n].Points.AddXY(X1, Y1);
                }
                n++;
            }
        }

            #region sin,cos,l1,l2,l3,m1,m2,m3,n1,n2,n3,clear,drawxyz
            double sin(double x)
        {
            return Math.Sin(x * Math.PI / 180);
        }
        double cos(double x)
        {
            return Math.Cos(x * Math.PI / 180);
        }
        double l1()
        {
            return cos(a) * cos(c) - cos(b) * sin(a) * sin(c);
        }
        double m1()
        {
            return sin(a) * cos(c) + cos(b) * cos(a) * sin(c);
        }
        double n1()
        {
            return sin(b) * sin(c);
        }
        double l2()
        {
            return -cos(a) * sin(c) + cos(b) * sin(a) * cos(c);
        }
        double m2()
        {
            return -sin(a) * sin(c) + cos(b) * cos(a) * cos(c);
        }
        double n2()
        {
            return sin(b) * cos(c);
        }
        double l3()
        {
            return sin(b) * sin(a);
        }
        double m3()
        {
            return -sin(b) * cos(a);
        }
        double n3()
        {
            return cos(b);
        }
        void drawxyz()
        {
            double L = N / 2; //axis length

            //z
            chart1.Series[2 * N].Points.AddXY(0, 0);
            chart1.Series[2 * N].Points.AddXY(l3() * L, m3() * L);
            chart1.Series[2 * N].Points[1].Label = "Z";
            //chart1.Series[2 * N * 3].Points[0].Label = "0";
            //x
            chart1.Series[2 * N + 1].Points.AddXY(0, 0);
            chart1.Series[2 * N + 1].Points.AddXY(l1() * L, m1() * L);
            chart1.Series[2 * N + 1].Points[1].Label = "X";
            //y
            chart1.Series[2 * N + 2].Points.AddXY(0, 0);
            chart1.Series[2 * N + 2].Points.AddXY(l2() * L, m2() * L);
            chart1.Series[2 * N + 2].Points[1].Label = "Y";
        }
        void clear()
        {
            for (int i = 0; i < chart1.Series.Count; i++) chart1.Series[i].Points.Clear();
        }

        #endregion

        void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (N > 3)
                {
                    N -= 2;
                    Z = N / 3; 
                    chart1.Series.Clear();
                    update();
                    init();
                }
            }
            else
            {
                if (N < 51)
                {
                    N += 2;
                    Z = N / 3; 
                    chart1.Series.Clear();
                    update();
                    init();
                }
            }

            this.Text = "a=" + a.ToString("0.00") + " b=" + b.ToString("0.00") + " c=" + c.ToString("0.00") + " N=" + N.ToString();
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        { 
                if (onmove)
                {
                    if ((startpos.Y - e.Y) < 0) b--;
                    if ((startpos.Y - e.Y) > 0) b++;
                    if ((startpos.X - e.X) < 0) c--;
                    if ((startpos.X - e.X) > 0) c++;

                    if (b > 359) b = 0;
                    if (c > 359) c = 0;
                    if (b < 0) b = 359;
                    if (c < 0) c = 359;

                    drawscene();

                    this.Text = "a=" + a.ToString("0.00") + " b=" + b.ToString("0.00") + " c=" + c.ToString("0.00") + " N=" + N.ToString();
                }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {     
                drawscene();     
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < 2*N; i++)
                    chart1.Series[i].Color = colorDialog1.Color;
                for (int i = 2 * N + 3; i < 4 * N + 3; i++)
                    chart1.Series[i].Color = colorDialog1.Color;
            }
                
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            for (int i = 0; i < 2 * N; i++)
                chart1.Series[i].BorderWidth = trackBar1.Value;
            for (int i = 2 * N + 3; i < 4 * N + 3; i++)
                chart1.Series[i].BorderWidth = trackBar1.Value;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Chart1_MouseUp_1(object sender, MouseEventArgs e)
        {
                if (e.Button == MouseButtons.Left) onmove = false;
        }

        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        { 
                if (e.Button == MouseButtons.Left)
                {
                    onmove = true;
                    startpos = e.Location;
                }  
        }             
    }
}
