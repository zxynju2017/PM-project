using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Hotel_Management;

namespace Hotel_Management
{
    public partial class Login : Form
    {
        public bool is_stuff = false;
        public static string currentUserAccount;
        public static string currentUserName;
        public Login()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "office2007.ssk";
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = this.textBox1.Text;
            string psd = this.textBox2.Text;
            string commstr = "select * from account where customer_id=@username and password=dbo.MD5(@psd,32)";
            SqlCommand comm = new SqlCommand(commstr);
            comm.Parameters.AddWithValue("@username", username);
            comm.Parameters.AddWithValue("@psd", psd);
            DataSet ds = Hotel_Management.SQLconnect.GetData(comm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("登陆成功！");
                currentUserAccount = username;
                currentUserName = ds.Tables[0].Rows[0]["user_name"].ToString();
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.textBox1.Focus();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SingUp singUp = new SingUp();
            singUp.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string username = this.textBox3.Text;
            string psd = this.textBox4.Text;
            string commstr = "select * from [stuff] where recept_id=@username and password=dbo.MD5(@psd,32)";
            SqlCommand comm = new SqlCommand(commstr);
            comm.Parameters.AddWithValue("@username", username);
            comm.Parameters.AddWithValue("@psd", psd);
            DataSet ds = Hotel_Management.SQLconnect.GetData(comm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("登陆成功！");
                this.DialogResult = DialogResult.OK;
                currentUserAccount = username;
                is_stuff = true;
                this.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
                this.textBox3.Text = "";
                this.textBox4.Text = "";
                this.textBox3.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
