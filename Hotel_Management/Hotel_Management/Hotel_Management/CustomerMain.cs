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
    public partial class CustomerMain : Form
    {
        public CustomerMain()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "office2007.ssk";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CustomerMain_Load(object sender, EventArgs e)
        {
            customerMain_init();
        }

        private void customerMain_init()
        {
            string commstr = "select type from room_type";
            SqlCommand comm = new SqlCommand(commstr);
            DataSet ds = Hotel_Management.SQLconnect.GetData(comm);
            this.comboBox1.Items.Add("全部");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.comboBox1.Items.Add(ds.Tables[0].Rows[i]["type"]);
            }
            this.comboBox1.SelectedIndex = 0;
            this.label6.Text = Login.currentUserAccount;
            this.label7.Text = Login.currentUserName;

            string commstr2 = "select name as 姓名,sex as 性别,tel as 电话,customer.id as 身份证号 from customer,account_customer where customer.id=account_customer.id"
                + " and customer_id=@Account";
            SqlCommand comm2 = new SqlCommand(commstr2);
            comm2.Parameters.AddWithValue("@Account", Login.currentUserAccount);
            DataSet ds2 = Hotel_Management.SQLconnect.GetData(comm2);

            DataView dv2 = new DataView(ds2.Tables[0]);
            this.dataGridView2.DataSource = dv2;

            this.dateTimePicker2.Value = DateTime.Now.AddDays(1);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
            DateTime dt1 = this.dateTimePicker1.Value.Date;
            DateTime dt2 = this.dateTimePicker2.Value.Date;
            TimeSpan ts= dt2 - dt1;
            int days = ts.Days;
            if(days<=0)
            {
                MessageBox.Show("离店日期不能早于或等于入住日期！");
                this.dateTimePicker1.Value = DateTime.Now;
                this.dateTimePicker2.Value = DateTime.Now.AddDays(1);
                days = 1;
            }
            this.label3.Text = "共" + days.ToString() + "天";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rtype = this.comboBox1.SelectedItem.ToString().Trim();
            
            if (rtype == "全部")
            {
                string commstr = "select room_id as 房间号,room.type as 房型,price as 价格,remark as " +
                    "备注 from room,room_type where room.type=room_type.type and room_id not in " +
                    "(select room_id from [order] " +
                    "where ((convert(date,@inDate) between convert(date,in_date) and convert(date,out_date)) " +
                    "or (convert(date,@outDate) between convert(date,in_date) and convert(date,out_date))) " +
                    "and [type]!='canceled')";
                SqlCommand comm = new SqlCommand(commstr);
                comm.Parameters.AddWithValue("@inDate", this.dateTimePicker1.Value.ToString("yyyyMMdd"));
                comm.Parameters.AddWithValue("@outDate", this.dateTimePicker2.Value.ToString("yyyyMMdd"));
                DataSet ds = Hotel_Management.SQLconnect.GetData(comm);

                DataView dv = new DataView(ds.Tables[0]);
                this.dataGridView1.DataSource = dv;
            }
            else
            {
                string commstr = "select room_id as 房间号,room.type as 房型,price as 价格,remark as 备注 from room,room_type where room.type=room_type.type and room_id not in" +
                    "(select room_id from [order] " +
                    "where ((convert(date,@inDate) between convert(date,in_date) and convert(date,out_date)) " +
                    "or (convert(date,@outDate) between convert(date,in_date) and convert(date,out_date))) " +
                    "and [type]!='canceled') " + 
                    "and room.type=@rtype";
                SqlCommand comm = new SqlCommand(commstr);
                comm.Parameters.AddWithValue("@rtype", rtype);
                comm.Parameters.AddWithValue("@inDate", this.dateTimePicker1.Value.ToString("yyyyMMdd"));
                comm.Parameters.AddWithValue("@outDate", this.dateTimePicker2.Value.ToString("yyyyMMdd"));
                DataSet ds = Hotel_Management.SQLconnect.GetData(comm);

                DataView dv = new DataView(ds.Tables[0]);
                this.dataGridView1.DataSource = dv;
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt1 = this.dateTimePicker1.Value;
            DateTime dt2 = this.dateTimePicker2.Value;
            if (this.dataGridView1.SelectedCells[0].ColumnIndex != 0) MessageBox.Show("请选择房间号！");
            else
            {

                //string roomNumber = this.dataGridView1.SelectedRows[0].Cells["房间号"].Value.ToString();
                string roomNumber = this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["房间号"].Value.ToString();
                double roomPrice = Convert.ToDouble(this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["价格"].Value.ToString());
                TimeSpan ts = dt2 - dt1;
                int days = ts.Days + 1;
                OrderConfirm orderConfirm = new OrderConfirm(roomNumber, roomPrice, dt1, dt2, days);
                orderConfirm.ShowDialog();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.ShowDialog();
            customerMain_init();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dataGridView2.SelectedCells[0].ColumnIndex != 0)
            {
                MessageBox.Show("请选择被删除人姓名！");
            }
            else
            {
                DialogResult dr = MessageBox.Show("确认删除该条信息？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string cID = this.dataGridView2.Rows[this.dataGridView2.SelectedCells[0].RowIndex].Cells["身份证号"].Value.ToString();
                    //string cID = this.dataGridView2.SelectedRows[0].Cells["身份证号"].Value.ToString();
                    string commstr = "delete account_customer where customer_id = @currentUserAccount and id = @cID";
                    SqlCommand comm = new SqlCommand(commstr);
                    comm.Parameters.AddWithValue("@currentUserAccount", Login.currentUserAccount);
                    comm.Parameters.AddWithValue("@cID", cID);
                    if (SQLconnect.InsertData(comm))
                    {
                        MessageBox.Show("删除成功！");
                        customerMain_init();
                    }
                    else MessageBox.Show("删除失败！");
                }
                else
                {

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            myOrders myorders = new myOrders('0');
            myorders.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            myOrders myorders = new myOrders('1');
            myorders.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            myOrders myorders = new myOrders('2');
            myorders.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
    }
}
