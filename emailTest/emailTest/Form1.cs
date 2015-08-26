using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace emailTest
{


    public partial class Form1 : Form
    {

       

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tb_uName.Text.Length > 0 && tb_uPwd.Text.Length > 0)
            {
                loadMail_btn.Enabled = false;

                connectGmail cG = new connectGmail(this);
                cG.LoadEmail(tb_uName.Text, tb_uPwd.Text, dataGridView1, progressBar1);

                loadMail_btn.Enabled = true;
            }
            else {
                MessageBox.Show("請輸入帳號密碼","警告");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_uName.Text = "";
            tb_uPwd.Text = "";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            dataGridView1.SetBounds(dataGridView1.Location.X, dataGridView1.Location.Y, this.Size.Width-40, this.Size.Height-113);
        }
    }
}
