using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _4._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Point;
            chart1.Series[2].ChartType = SeriesChartType.Point;
            for (double F = 1; F < 1000; F++) 
            {
               
                
                    chart1.Series[0].Points.AddXY(F, D = 1.0 / F);

                
            }
            
        }
        double D;
        int F1=0 ;
        int F2 = 1000;
        private void timer1_Tick(object sender, EventArgs e)
        {
            F1+= 1;
            
            chart1.Series[1].Points.Clear();
            chart1.Series[1].Points.AddXY(F1, D = 1.0/ F1);
            if (F1>1000) 
            {
               
                timer1.Stop();
            }
            
            F2 -= 1;
            chart1.Series[2].Points.Clear();
            chart1.Series[2].Points.AddXY(F2, D = 1.0 / F2);
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            timer1.Start();
           
            
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            timer1.Interval = hScrollBar1.Value;
        }
    }
}
