using System;
using System.Collections;
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
          //  CountBooking();
         //   GetCustomer();
            bookedcheck();
           
        }
        void populatenamecombo()
        {
            CusNameCb.Items.Clear();
           
            if(Program.guest!=null)
            foreach(Guest gue in Program.guest)
            {
                if(gue!=null)
                CusNameCb.Items.Add(gue.FullName);
            }
           
           
        }

        private void R1_Paint(object sender, PaintEventArgs e)
        {
            RoomNumber = 1;
        }

        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        int free, Booked;
        int BPer, FreePer;

        void bookedcheck()
        {
            for (int i = 1; i <= 20; i++)
            {
                string panelName = "R" + i;
                Panel roomPanel = this.Controls[panelName] as Panel;
                if (roomPanel != null)
                {
                    if (Program.list!=null&&Program.list.Isbooked(i))
                    {
                        roomPanel.BackColor = Color.LightGray;

                        // Make it unclickable
                        roomPanel.Enabled = false;
                    }
                    else
                    {
                        roomPanel.BackColor =Color.LightCyan;

                        // Make it clickable
                        roomPanel.Enabled = true;
                    }
                }
            }
        }
        private void CountBooked()
        {
            string Status = "occupied";
            if (Program.list != null)
            {
                free = 20 - Program.list.TotalBooking;
                Booked = Program.list.TotalBooking;
                BPer = (Booked / 20) * 100;
                FreePer = (free / 20) * 100;
                BLbl.Text = Booked + " Booked Rooms";//Number of booked rooms(booked label)
                AVllbl.Text = free + " Free Rooms";//NUmber of free rooms(availible label)
                AvLbl1.Text = free + "";//this shows the number of free rooms from the freeRoomsProgressbar
                BProgress.Value = BPer;//shows the progress bar amount for booked spaces
                AVLProgress.Value = FreePer;//shows the progress bar amount for free spaces
                FreeRoomsProgress.Value = FreePer;//shows the progress bar amount of the of free spaces left
            }
        }
        private void CountCustomers()
        { 
           
            if(Program.list!=null)
            CustNumLbl.Text = Program.list.TotalBooking.ToString() + " Customers";
            
        
        }
        
        int RoomNumber = 0;
      
        
        string RType;
        int RC;
        private void GetRoomType()
        {
            Con.Open();
            string Query = "select * from Rooms where RoomNumber=" + RoomNumber + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                RType = dr["Type"].ToString();
                RC = Convert.ToInt32(dr["Price"].ToString());
            }
            Con.Close();
        }
       
        private void Reset()
        {
            RType = "";
            RC = 0;
            RoomNumber = 0;
        }
       
        private void BookBtn_Click(object sender, EventArgs e)
        {
            Guest singleguest=null;

            if (CusNumofDaysTb.Text == "" || RoomNumber == 0||CusNameCb.Text=="")
            {
                MessageBox.Show("Select a Room and a Customer");
            } else
            {
                foreach(Guest g in Program.guest)
                {
                   
                    if(g != null&&g.FullName== CusNameCb.Text)
                    {
                    singleguest= new Guest(g.FullName,g.PhoneNumber,g.Dob,g.gender);

                    }
                }

                try
                {
                    
                    Program.list.AddBooking(RoomNumber,singleguest, Program.list.getPrice(RoomNumber) * int.Parse(CusNumofDaysTb.Text));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                
            }
        }

        private void CusIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //GetCusName();
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
         Program.   objcustomer.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
          Program.  objbooking.Show();
            this.Hide();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Program.objlogin.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
       
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
           Program. objuser.Show();
            this.Hide();
        }

        private void AVllbl_Click(object sender, EventArgs e)
        {

        }

        private void DashBoard_Enter(object sender, EventArgs e)
        {
          //  populatenamecombo();
        }

        private void DashBoard_VisibleChanged(object sender, EventArgs e)
        {
            populatenamecombo();

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
           // Program.list = new LinkedProgram.list();
            populatenamecombo();

        }
    }
}
