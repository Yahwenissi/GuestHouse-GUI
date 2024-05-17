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
       
    
        public Customers()
        {
            InitializeComponent();
            showCustomers();
        }

        void loadtable()
        {
            if (Program. guest != null && Program.guest.Length > track)
            {
                newCustomertbl.Rows.Clear();
                for (int i = 0; i < Program.guest.Length; i++)
                {
                    if (Program.guest[i] != null && Program.guest[i].status == "pending")
                    {
                        newCustomertbl.Rows.Add(Program.guest[i].FullName, Program.guest[i].Dob, Program.guest[i].Age, Program.guest[i].gender, Program.guest[i].PhoneNumber);
                    }
                }
            }
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            loadtable();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        private void showCustomers()
        {
            Con.Open();
            string Query = "select * from GuestView";
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
    //        key = 0;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusPhoneTb.Text == "" || CusGenCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                if (track >= Program.guest.Length)
                {
                    MessageBox.Show("Guest List is full");
                    return;
                }

                Program.guest[track] = new Guest(CusNameTb.Text, CusPhoneTb.Text, CusDOB.Value.Date.ToString(), CusGenCb.SelectedItem.ToString());
                track += 1;
                MessageBox.Show("Customer Saved");
                loadtable();
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
           /*     try
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
                }*/
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
      /*      if (key == 0)
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
            }*/
        }
    //    int key = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }







        private void pictureBox4_Click(object sender, EventArgs e)
        {
         //   objcustomer.Show();
         //   this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
           Program. objbooking.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Program.objuser.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Program.objdash.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Program.objlogin.Show();
            this.Hide();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CustomersDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CusNameTb.Text = CustomersDGV.SelectedRows[0].Cells[0].Value.ToString();
            CusPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[4].Value.ToString();
            CusGenCb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
            CusDOB.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();

            if (CusNameTb.Text == "")
            {
                //     key = 0;
            }
            else
            {
                //      key = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
