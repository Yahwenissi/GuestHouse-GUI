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
        int track = 0,roomnum=0;
       
    
        public Customers()
        {
            InitializeComponent();
            showCustomers();
        }

        void loadtable()
        {
            showCustomers();
            newCustomertbl.Rows.Clear();

            if (Program. guest != null && Program.guest.Length > track)
            {
                for (int i = 0; i < Program.guest.Length; i++)
                {
                    if (Program.guest[i] != null && Program.guest[i].status == "pending")
                    {
                        newCustomertbl.Rows.Add(Program.guest[i].FullName, Program.guest[i].Dob, Program.guest[i].gender, Program.guest[i].PhoneNumber);
                    }
                }
            }
        }

     
        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        private void showCustomers()
        {
            CustomersDGV.Rows.Clear();
            Booking curr = Program.list.Head;
            while (curr != null)
            {
                if (curr.Guest != null)
                    CustomersDGV.Rows.Add(curr.Guest.FullName, curr.Guest.Age,curr.Guest.Dob, curr.Guest.gender, curr.Guest.PhoneNumber,curr.Room.RoomNumber);
                curr = curr.Next;
            }

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

                Program.guest[track] = new Guest(CusNameTb.Text, CusPhoneTb.Text, CusDOB.Value.Date.ToString(), CusGenCb.Text);
                track += 1;
             //   MessageBox.Show("Customer Saved");
                loadtable();
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            DateTime dob;
            if (CusNameTb.Text == "" && CusPhoneTb.Text == "" && CusGenCb.SelectedIndex == -1 && CusDOB.Text == DateTime.Now.ToString()||roomnum==0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                Guest gue = new Guest();
                if (CusNameTb.Text != "" && CusPhoneTb.Text == "" && CusGenCb.SelectedIndex == -1 && CusDOB.Text == DateTime.Now.ToString())
                    gue.FullName = CusNameTb.Text;
                if (CusNameTb.Text == "" && CusPhoneTb.Text != "" && CusGenCb.SelectedIndex == -1 && CusDOB.Text == DateTime.Now.ToString())
                    gue.PhoneNumber= CusPhoneTb.Text;
                if (CusNameTb.Text == "" && CusPhoneTb.Text == "" && CusGenCb.SelectedIndex != -1 && CusDOB.Text == DateTime.Now.ToString())
                    gue.gender=CusGenCb.Text;
                if (CusNameTb.Text == "" && CusPhoneTb.Text == "" && CusGenCb.SelectedIndex == -1 && CusDOB.Text != DateTime.Now.ToString())
                    gue.Dob=CusDOB.Text;
                 if (CusNameTb.Text != "" && CusPhoneTb.Text != "" && CusGenCb.SelectedIndex != -1 && CusDOB.Text != DateTime.Now.ToString())
                {
                    gue.FullName = CusNameTb.Text;
                    gue.PhoneNumber = CusPhoneTb.Text;
                    gue.gender = CusGenCb.Text;
                   
                    if (DateTime.TryParse(CusDOB.Text, out dob))
                    {
                        gue.Dob = dob.ToString("yyyy/MM/dd");
                      
                        gue.Age=CalculateAge(dob);
                        
                    }
                    



                }


                Program.list.ModifyGuest(gue,roomnum);
                loadtable();
                showCustomers();
            }
        }
        public int CalculateAge(DateTime dob)
        {
            // Get the current date
            DateTime currentDate = DateTime.Today;

            // Calculate age
            int age = currentDate.Year - dob.Year;

            // Check if the birthday has passed this year
            if (dob > currentDate.AddYears(-age))
                age--;

            return age;
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
            try
            {
                if (CustomersDGV.Rows.Count > 0)
                {
                   // MessageBox.Show("List is Empty!");
                }
                else
                {
                    CusNameTb.Text = CustomersDGV.SelectedRows[0].Cells[0].Value.ToString();
                    CusPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[4].Value.ToString();
                    CusGenCb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
                    CusDOB.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();
                    roomnum = int.Parse(CustomersDGV.SelectedRows[0].Cells[5].Value.ToString());

                }
            }catch(NullReferenceException ex)
            {
                MessageBox.Show("List is Empty!");
            }
        }

        private void Customers_Load_1(object sender, EventArgs e)
        {
            loadtable();
        }
    }
}
