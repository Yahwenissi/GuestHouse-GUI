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
using System.Collections;

namespace GuestHouse_GUI
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            showUsers();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        private void showUsers()
        {
            Con.Open();
            string Query = "select * from UserTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query,Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UsersDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            if(UnameTb.Text == "" || UpasswordTb.Text == "" || UpasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            } else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into UserTbl(Uname,Uphone,Upass) values(@UN,@UP,@UPA)", Con);
                    cmd.Parameters.AddWithValue("@UN", UnameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UphoneTb.Text);
                    cmd.Parameters.AddWithValue("@UPA", UpasswordTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Saved");
                    Con.Close();
                    showUsers();
                    Reset();

                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        int key = 0;
        private void UsersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UnameTb.Text = UsersDGV.SelectedRows[0].Cells[1].Value.ToString();
            UphoneTb.Text = UsersDGV.SelectedRows[0].Cells[2].Value.ToString();
            UpasswordTb.Text = UsersDGV.SelectedRows[0].Cells[3].Value.ToString();
            if(UnameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(UsersDGV.SelectedRows[0].Cells[0].Value.ToString());
            } 
                

        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || UpasswordTb.Text == "" || UpasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update UserTbl set Uname = @UN, Uphone = @UP, Upass = @UPA where UId = @Ukey", Con);
                    cmd.Parameters.AddWithValue("@UN", UnameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UphoneTb.Text);
                    cmd.Parameters.AddWithValue("@UPA", UpasswordTb.Text);
                    cmd.Parameters.AddWithValue("@Ukey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated");
                    Con.Close();
                    showUsers();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void Reset()
        {
            UnameTb.Text = "";
            UphoneTb.Text = "";
            UpasswordTb.Text = "";
            key = 0;
        }
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select User");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from UserTbl where UId = @Ukey", Con);
                    cmd.Parameters.AddWithValue("@Ukey", key);
                    
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted");
                    Con.Close();
                    showUsers();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Booking obj = new Booking();
            obj.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
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
    }
}
