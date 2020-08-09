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

namespace Hotel_Management
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "office2007.ssk";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.textBox1.Text=="")
            {
                MessageBox.Show("请填写入住人姓名！");
            }
            else if(this.textBox2.Text == "")
            {
                MessageBox.Show("请填写入住人身份证号！");
            }
            else if(this.textBox3.Text == "")
            {
                MessageBox.Show("请填写入住人联系方式！");
            }
            else
            {
                string name = this.textBox1.Text;
                char sex = 'M';
                if (this.radioButton2.Checked) sex = 'F';
                string ID = this.textBox2.Text;
                string tel = this.textBox3.Text;

                string commstr = "insert customer values(@ID,@name,@sex,@tel)";
                string commstr2 = "insert account_customer values(@account,@ID)";
                SqlCommand comm = new SqlCommand(commstr);
                comm.Parameters.AddWithValue("@ID", ID);
                comm.Parameters.AddWithValue("@name", name);
                comm.Parameters.AddWithValue("@sex", sex);
                comm.Parameters.AddWithValue("@tel", tel);
                SqlCommand comm2 = new SqlCommand(commstr2);
                comm2.Parameters.AddWithValue("@account", Login.currentUserAccount);
                comm2.Parameters.AddWithValue("@ID", ID);
                SQLconnect.InsertData(comm);
                if (SQLconnect.InsertData(comm2))
                {
                    MessageBox.Show("添加成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("该身份已存在！");
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.textBox3.Text = "";
                    this.radioButton2.Checked = false;
                    this.radioButton1.Checked = true;
                    this.textBox1.Focus();
                }
            }
            
        }

        private void AddCustomer_Load(object sender, EventArgs e)
        {

        }
    }
}
