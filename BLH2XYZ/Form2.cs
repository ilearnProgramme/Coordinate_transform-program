using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLHtoXYZ
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        

        private void Form2_Load(object sender, EventArgs e)
        {
            if (imageList1.Images.Count > 0)
            {
                pictureBox1.Image = imageList1.Images[0];
            }
            else
                MessageBox.Show("图片显示错误！");
        }


        //Bitmap curBitmap = null;
        //curBitmap =(Bitmap)Image.FromFile("");
    //    try
    //{

    //}
    //    catch(Exception exp) { MessageBox.Show(exp.Message);}
   } 
}
