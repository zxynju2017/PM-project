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
    public partial class myOrders : Form
    {
        char flag;
        public myOrders()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "office2007.ssk";
        }
        public myOrders(char f)
        {
            InitializeComponent();
            flag = f;
        }

        private void myOrders_init()
        {
            if (flag == '0')
            {
                this.tabControl1.SelectedIndex = 0;
            }
            else if (flag == '1')
            {
                this.tabControl1.SelectedIndex = 1;
            }
            else
            {
                this.tabControl1.SelectedIndex = 2;
            }

            string commstr = "select order_id as 订单号,in_date as 入住日期,out_date as 离店日期," +
                "room.type as 房型,price as 价格 from [order],room " +
                "where [order].room_id=room.room_id and [order].type='will' and customer_id=@Account";
            SqlCommand comm = new SqlCommand(commstr);
            comm.Parameters.AddWithValue("@Account", Login.currentUserAccount);
            DataSet ds = SQLconnect.GetData(comm);

            DataView dv = new DataView(ds.Tables[0]);
            this.dataGridView1.DataSource = dv;

            string commstr2 = "select order_id as 订单号,in_date as 入住日期,out_date as 离店日期," +
                "room.type as 房型,price as 价格 from [order],room " +
               "where [order].room_id=room.room_id and [order].type='ing' and customer_id=@Account";
            SqlCommand comm2 = new SqlCommand(commstr2);
            comm2.Parameters.AddWithValue("@Account", Login.currentUserAccount);
            DataSet ds2 = SQLconnect.GetData(comm2);

            DataView dv2 = new DataView(ds2.Tables[0]);
            this.dataGridView2.DataSource = dv2;

            string commstr3 = "select order_id as 订单号,in_date as 入住日期,out_date as 离店日期," +
                "room.type as 房型,price as 价格,[order].type as 订单状态 from [order],room " +
               "where [order].room_id=room.room_id and customer_id=@Account";
            SqlCommand comm3 = new SqlCommand(commstr3);
            comm3.Parameters.AddWithValue("@Account", Login.currentUserAccount);
            DataSet ds3 = SQLconnect.GetData(comm3);

            DataView dv3 = new DataView(ds3.Tables[0]);
            this.dataGridView3.DataSource = dv3;
        }

        private void myOrders_Load(object sender, EventArgs e)
        {
            myOrders_init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex != 0) MessageBox.Show("请选择要取消的订单号！");
            else
            {
                string oid = this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["订单号"].Value.ToString();
                //string oid = dataGridView1.SelectedRows[0].Cells["订单号"].Value.ToString();
                string commstr = "update [order] set [type]='canceled' where order_id=@oid";
                SqlCommand comm = new SqlCommand(commstr);
                comm.Parameters.AddWithValue("@oid", oid);
                if (SQLconnect.InsertData(comm))
                {
                    MessageBox.Show("取消成功！");
                    myOrders_init();
                }
                else MessageBox.Show("取消失败！");
            }
        }
    }
}
