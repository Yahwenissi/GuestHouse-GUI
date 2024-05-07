using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestHouse_GUI
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
            CountBooked();
            CountCustomers();
            CountBooking();
            GetCustomer();
        }

        private void R1_Paint(object sender, PaintEventArgs e)
        {
            RoomNumber = 1;
        }

        SqlConnection Con = new SqlConnection(@"Data Source=NESSI-G\SQL2022;Initial Catalog=GuestHouseDb;Integrated Security=True");
        int free, Booked;
        int BPer, FreePer;
        private void CountBooked()
        {
            string Status = " Booked";
            
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from RoomTbl where RStatus = '"+Status+"'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            free = 20 - Convert.ToInt32(dt.Rows[0][0].ToString());
            Booked = Convert.ToInt32(dt.Rows[0][0].ToString());
            BPer = (Booked / 20) * 100;
            FreePer = (free / 20) * 100;
            BLbl.Text = dt.Rows[0][0].ToString()+" Booked Rooms";//Number of booked rooms(booked label)
            AVllbl.Text = free + " Free Rooms";//NUmber of free rooms(availible label)
            AvLbl1.Text = free + "";//this shows the number of free rooms from the freeRoomsProgressbar
            BProgress.Value = BPer;//shows the progress bar amount for booked spaces
            AVLProgress.Value = FreePer;//shows the progress bar amount for free spaces
            FreeRoomsProgress.Value = FreePer;//shows the progress bar amount of the of free spaces left

            Con.Close();
        }
        private void CountCustomers()
        { 
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from CustomerTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            
            CustNumLbl.Text = dt.Rows[0][0].ToString() + " Customers";
            
            Con.Close();
        }
        private void CountBooking()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from BookingTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            BookedLbl.Text = dt.Rows[0][0].ToString() + " Bookings";

            Con.Close();
        }
        int RoomNumber = 0;
        private void GetCustomer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CusId from CustomerTbl", Con);
            SqlDataReader rdr;
            rdr=cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CusId", typeof(int));
            dt.Load(rdr);
            CusIdCb.ValueMember = "CusId";
            CusIdCb.DataSource = dt;
            Con.Close() ;
        }
        string RType;
        int RC;
        private void GetRoomType()
        {
            Con.Open();
            string Query = "select * from RoomTbl where RId=" + RoomNumber + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                RType = dr["RType"].ToString();
                RC = Convert.ToInt32(dr["RCost"].ToString());
            }
            Con.Close();
        }
        private void GetCusName()
        {
            Con.Open();
            string Query = "select * from CustomerTbl where CusId=" + CusIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                CusNameTb.Text = dr["CusName"].ToString();
            }
            Con.Close();
        }
        private void Reset()
        {
            RType = "";
            RC = 0;
            RoomNumber = 0;
        }
        private void UpdateRoom()
        {
            string Status = "Booked";
            try
                {

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update RoomTbl set RStatus = @RS where RId = @RKey", Con);
                    cmd.Parameters.AddWithValue("@RS", Status);
                    cmd.Parameters.AddWithValue("@RKey", RoomNumber);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Updated!");
                    Con.Close();
                    Reset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
        }
        private void BookBtn_Click(object sender, EventArgs e)
        {
            if(CusNameTb.Text == "" || RoomNumber == 0)
            {
                MessageBox.Show("Select a Room and a Customer");
            } else
            {
                try
                {
                    
                    GetRoomType();
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BookingTbl(CusId,CusName, RId, RNum, RType, BCost) values(@CI,@CN,@RI,@RN,@RT,@RC)", Con);
                    cmd.Parameters.AddWithValue("@CI", CusIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@RI", RoomNumber);
                    cmd.Parameters.AddWithValue("@RN", RoomNumber);
                    cmd.Parameters.AddWithValue("@RT", RType);
                    cmd.Parameters.AddWithValue("@RC", RC);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Booked");
                    Reset();
                    Con.Close();
                    UpdateRoom();
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CusIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCusName();
        }

        private void R2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R4_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R5_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R6_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R7_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R8_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R9_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R10_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R11_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R12_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R13_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void R14_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R15_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R16_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R17_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R18_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R19_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R20_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void R1_Click(object sender, EventArgs e)
        {
            RoomNumber = 1;
        }

        private void R2_Click(object sender, EventArgs e)
        {
            RoomNumber = 2;
        }

        private void R3_Click(object sender, EventArgs e)
        {
            RoomNumber = 3;
        }

        private void R4_Click(object sender, EventArgs e)
        {
            RoomNumber = 4;
        }

        private void R5_Click(object sender, EventArgs e)
        {
            RoomNumber = 5;
        }

        private void R6_Click(object sender, EventArgs e)
        {
            RoomNumber = 6;
        }

        private void R7_Click(object sender, EventArgs e)
        {
            RoomNumber = 7;
        }

        private void R8_Click(object sender, EventArgs e)
        {
            RoomNumber = 8;
        }

        private void R9_Click(object sender, EventArgs e)
        {
            RoomNumber = 9;
        }

        private void R10_Click(object sender, EventArgs e)
        {
            RoomNumber = 10;
        }

        private void R11_Click(object sender, EventArgs e)
        {
            RoomNumber = 11;
        }

        private void R12_Click(object sender, EventArgs e)
        {
            RoomNumber = 12;
        }

        private void R13_Click(object sender, EventArgs e)
        {
            RoomNumber = 13;
        }

        private void R14_Click(object sender, EventArgs e)
        {
            RoomNumber = 14;
        }

        private void R15_Click(object sender, EventArgs e)
        {
            RoomNumber = 15;
        }

        private void R16_Click(object sender, EventArgs e)
        {
            RoomNumber = 16;
        }

        private void R17_Click(object sender, EventArgs e)
        {
            RoomNumber = 17;
        }

        private void R18_Click(object sender, EventArgs e)
        {
            RoomNumber = 18;
        }

        private void R19_Click(object sender, EventArgs e)
        {
            RoomNumber = 19;
        }

        private void R20_Click(object sender, EventArgs e)
        {
            RoomNumber = 20;
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

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void AVllbl_Click(object sender, EventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {

        }
    }
}
