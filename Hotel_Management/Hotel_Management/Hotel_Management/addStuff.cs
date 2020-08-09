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
    public partial class addStuff : Form
    {
        public addStuff()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addStuff_Load(object sender, EventArgs e)
        {
            string commstr = "select posi_chs from position";
            SqlCommand comm = new SqlCommand(commstr);
            DataSet ds = Hotel_Management.SQLconnect.GetData(comm);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.comboBox1.Items.Add(ds.Tables[0].Rows[i]["posi_chs"]);
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.textBox1.Text=="")
            {
                MessageBox.Show("请填写员工名字！");
            }
            else if(this.textBox2.Text=="")
            {
                MessageBox.Show("请填写员工号！");
            }
            else if(this.textBox3.Text == "")
            {
                MessageBox.Show("请填写员工密码！");
            }
            else
            {
                string name = this.textBox1.Text;
                string ID = this.textBox2.Text;
                string posi_chs = this.comboBox1.SelectedItem.ToString();
                string commstr = "select position from position where posi_chs=@posi_chs";
                SqlCommand comm = new SqlCommand(commstr);
                comm.Parameters.AddWithValue("@posi_chs", posi_chs);
                DataSet ds = new DataSet();
                ds = SQLconnect.GetData(comm);
                string position = ds.Tables[0].Rows[0]["position"].ToString();
                string psd = this.textBox3.Text;

                string commstr2 = "insert [stuff] values(@ID,@name,@psd,@position)";
                SqlCommand comm2 = new SqlCommand(commstr2);
                comm2.Parameters.AddWithValue("@ID", ID);
                comm2.Parameters.AddWithValue("@name", name);
                comm2.Parameters.AddWithValue("@psd", psd);
                comm2.Parameters.AddWithValue("@position", position);

                if (SQLconnect.InsertData(comm2))
                {
                    MessageBox.Show("添加成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("该员工已存在！");
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.textBox3.Text = "";
                    this.textBox1.Focus();
                }
            }
            
        }
    }
}
