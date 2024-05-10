using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestHouse_GUI
{
    public partial class Customers : Form
    {
        int track = 0;
        Guest[] guest;
        public Customers()
        {
            InitializeComponent();
            showCustomers();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            if (guest!=null&&guest.Length > track)
            {
                for (int i = 0; i < guest.Length; i++)
                {
                    if (guest[i].status == "Pending")
                    {

                    }
                }
            }
        }
        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        private void showCustomers()
        {
            Con.Open();
            string Query = "select * from Guests";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomersDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
            CusNameTb.Text = "";
            CusPhoneTb.Text = "";
            CusGenCb.SelectedIndex = -1;
            key = 0;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusPhoneTb.Text == "" || CusGenCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    guest[track] = new Guest(CusNameTb.Text, CusPhoneTb.Text, CusDOB.Value.Date.ToString(), CusGenCb.SelectedItem.ToString());

                    MessageBox.Show("Customer Saved");
                    Con.Close();
                    showCustomers();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusPhoneTb.Text == "" || CusGenCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CusName = @CN,CusPhone = @CP,CusGen = @CG ,CusDOB = @CD where CusId = @Ckey", Con);
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CusPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CG", CusGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CD", CusDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@Ckey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated!");
                    Con.Close();
                    showCustomers();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select Customer");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CusId = @Ckey", Con);
                    cmd.Parameters.AddWithValue("@Ckey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted");
                    Con.Close();
                    showCustomers();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        int key = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CusNameTb.Text = CustomersDGV.SelectedRows[0].Cells[1].Value.ToString();
            CusPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();
            CusGenCb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
            CusDOB.Text = CustomersDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (CusNameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Booking obj = new Booking();
            obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
