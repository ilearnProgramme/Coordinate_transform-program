using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BLHtoXYZ
{
    public struct point
    {
        public string name;
        public double B, L, H;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public const double PI=3.1415926;
        public static double dtoh(double du)
        {
            double hu, d, m, f;
            //du = du + 0.00000001;
            d = Math.Floor(du);
            f = Math.Floor((du - d) * 100);
            //m = Math.Floor((du - d - f / 100.0) * 10000);
            m =(du - d - f / 100.0) * 10000;
            du = d + f / 60.0 + m / 3600.0;
            hu = Math.Round(du * PI / 180.0, 8);
            return hu;
        }
        public static double htod(double hu)
        {
            double du, d, f, m;
            du = hu * 180.0 / PI;
            d = Math.Floor(du);
            f = Math.Floor((du - d) * 60);
            m = ((du - d) * 60 - f) * 60;
            du = d + f / 100.0 + m / 10000.0;
            return du;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FileStream file = File.Open(@"../../坐标转换（B卷）.txt", FileMode.Open); //@"E:\B卷-803组-贾俊何仲秋\BLHtoXYZ/坐标转换（B卷）.txt"
            StreamReader Reader = new StreamReader(file);
            double a, df, L0, b, e2, ep2, M0;
            double[] W = new double[19];
            double[] eta2 = new double[19];
            double[] t = new double[19];
            double[] N = new double[19];
            double[] M = new double[19];
            double[] X = new double[19];
            double[] Y = new double[19];
            double[] Z = new double[19];

            double[] x = new double[19];
            double[] y = new double[19];
            //读取文件
            string[] split1 = Reader.ReadLine().Split(',');
            a = Convert.ToDouble(split1[1]);
            string[] split2 = Reader.ReadLine().Split(',');
            df = Convert.ToDouble(split2[1]);
            string[] split3 = Reader.ReadLine().Split(',');
            L0 = Convert.ToDouble(split3[1]);
            Reader.ReadLine();
            point[] p = new point[19];
            for (int i = 0; i <= 18; i++)
            {
                string strLine = Reader.ReadLine();
                string[] split = strLine.Split(',');
                p[i].name = split[0];
                p[i].B = Convert.ToDouble(split[1]);
                p[i].B = dtoh(p[i].B);
                p[i].L = Convert.ToDouble(split[2]);
                p[i].L = dtoh(p[i].L);
                p[i].H = Convert.ToDouble(split[3]);
            }
            Reader.Close();
            file.Close();
            b = a - a / df;
            e2 = (a * a - b * b) / (a * a);
            ep2 = e2 / (1 - e2);
            M0 = a * (1 - e2);
            for (int i = 0; i <= 18; i++)
            {
                W[i] = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(p[i].B), 2));
                eta2[i] = ep2 * Math.Pow(Math.Cos(p[i].B), 2);
                t[i] = Math.Tan(p[i].B);
                N[i] = a / W[i];
                M[i] = a * (1 - e2) / Math.Pow(W[i], 3);
                X[i] = (N[i] + p[i].H) * Math.Cos(p[i].B) * Math.Cos(p[i].L);
                Y[i] = (N[i] + p[i].H) * Math.Cos(p[i].B) * Math.Sin(p[i].L);
                Z[i] = (N[i] * (1 - e2) + p[i].H) * Math.Sin(p[i].B);
            }
            //
            for (int i = 0; i < 19; i++)
            {
                double Ac, Bc, Cc, Dc, Ec, Fc;
                double alpha, beta, gama, delta, ex, phi;
                double XX, ll;

                double Bt = p[i].B;
                double Lt = p[i].L;
                double Nt = N[i];
                double tt = t[i];
                double eta2t = eta2[i];

                Ac = 1 + 3.0 / 4.0 * Math.Pow(e2, 1) + 45.0 / 64.0 * Math.Pow(e2, 2) + 175.0 / 256.0 * Math.Pow(e2, 3) + 11025.0 / 16384.0 * Math.Pow(e2, 4) + 43659.0 / 65536.0 * Math.Pow(e2, 5);
                Bc = 3.0 / 4.0 * Math.Pow(e2, 1) + 15.0 / 16.0 * Math.Pow(e2, 2) + 525.0 / 512.0 * Math.Pow(e2, 3) + 2205.0 / 2048.0 * Math.Pow(e2, 4) + 72756.0 / 65536.0 * Math.Pow(e2, 5);
                Cc = 15.0 / 64.0 * Math.Pow(e2, 2) + 105.0 / 256.0 * Math.Pow(e2, 3) + 2205.0 / 4096.0 * Math.Pow(e2, 4) + 10395.0 / 16384.0 * Math.Pow(e2, 5);
                Dc = 35.0 / 512.0 * Math.Pow(e2, 3) + 315.0 / 2048.0 * Math.Pow(e2, 4) + 31185.0 / 131072.0 * Math.Pow(e2, 5);
                Ec = 315.0 / 16384.0 * Math.Pow(e2, 4) + 3465.0 / 65536.0 * Math.Pow(e2, 5);
                Fc = 693.0 / 131072.0 * Math.Pow(e2, 5);

                alpha = Ac * M0;
                beta = -1.0 / 2.0 * Bc * M0;
                gama = 1.0 / 4.0 * Cc * M0;
                delta = -1.0 / 6.0 * Dc * M0;
                ex = 1.0 / 8.0 * Ec * M0;
                phi = 1.0 / 10.0 * Fc * M0;
                //子午线弧长
                XX = alpha * Bt + beta * Math.Sin(2 * Bt) + gama * Math.Sin(4 * Bt) + delta * Math.Sin(6 * Bt) + ex * Math.Sin(8 * Bt) + phi * Math.Sin(10 * Bt);
                ll = dtoh(htod(Lt) - L0);
                //计算辅助量
                double a0, a1, a2, a3, a4, a5, a6;

                a0 = XX;
                a1 = Nt * Math.Cos(Bt);
                a2 = 1.0 / 2.0 * Nt * Math.Pow(Math.Cos(Bt), 2) * tt;
                a3 = 1.0 / 6.0 * Nt * Math.Pow(Math.Cos(Bt), 3) * (1 - tt * tt + eta2t);
                a4 = 1.0 / 24.0 * Nt * Math.Pow(Math.Cos(Bt), 4) * (5 - tt * tt + 9.0 * eta2t + 4.0 * eta2t * eta2t) * tt;
                a5 = 1.0 / 120.0 * Nt * Math.Pow(Math.Cos(Bt), 5) * (5 - 18.0 * tt * tt + Math.Pow(tt, 4) + 14.0 * eta2t - 58.0 * eta2t * tt * tt);
                a6 = 1.0 / 720.0 * Nt * Math.Pow(Math.Cos(Bt), 6) * (61.0 - 58.0 * tt * tt + Math.Pow(tt, 4) + 270.0 * eta2t - 330.0 * eta2t * tt * tt) * tt;

                //高斯正反算
                double xt, yt;
                xt = a0 + a2 * ll * ll + a4 * Math.Pow(ll, 4) + a6 * Math.Pow(ll, 6);
                yt = a1 * ll + a3 * Math.Pow(ll, 3) + a5 * Math.Pow(ll, 5);

                yt = yt + 500000; //y坐标加500km
                //存储临时xt,yt到数组到x,y

                x[i] = xt;
                y[i] = yt;
            }
           int index;
            for (int i = 0; i < 18; i++)//将控制点的五个坐标存入五个数组
            {
                index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = p[i].name;
                dataGridView1.Rows[index].Cells[1].Value = X[i];
                dataGridView1.Rows[index].Cells[2].Value = Y[i];
                dataGridView1.Rows[index].Cells[3].Value = Z[i];
                dataGridView1.Rows[index].Cells[4].Value = p[i].B;
                dataGridView1.Rows[index].Cells[5].Value = p[i].L;
                dataGridView1.Rows[index].Cells[6].Value = p[i].H;
                dataGridView1.Rows[index].Cells[7].Value = x[i];
                dataGridView1.Rows[index].Cells[8].Value = y[i];
            }

            FileStream file2 = File.Open("result.txt", FileMode.Create, FileAccess.Write);
            StreamWriter Writer = new StreamWriter(file2);
            Writer.WriteLine("点名" + "        " + "X" + "                " + "Y" + "                " + "Z" + "               " + "B" + "               " + "L" + "           " + "H" + "             " + "x" + "             " + "y");
            for (int i = 0; i <= 18; i++)
            {
                Writer.WriteLine(p[i].name + "     " + X[i].ToString("f3") + "     " + Y[i].ToString("f3") + "     " + Z[i].ToString("f3") + "     " + p[i].B.ToString("f8") + "     " + p[i].L.ToString("f8") + "     " + p[i].H.ToString("f3") + "     " + x[i].ToString("f3") + "     " + y[i].ToString("f3"));
            }
            Writer.Close();
            file2.Close();


        }
      


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //散点图图形显示
        Form2 login = new Form2();
        private void button2_Click(object sender, EventArgs e)
        {
            login.Show();
        }
    }
}
