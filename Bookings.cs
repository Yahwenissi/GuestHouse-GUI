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
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            showBookings();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=RAFA;Initial Catalog=GuestHouse;Integrated Security=True");
        private void showBookings()
        {
           Booking curr=Program.list.Head;
            BookingDGV.Rows.Clear();

            while (curr!=null)
            {
                if(curr.Guest!=null) 
                BookingDGV.Rows.Add(curr.Guest.FullName,curr.Guest.Age,curr.Guest.gender,curr.Guest.PhoneNumber,curr.Guest.CheckInDate,curr.Guest.CheckOutDate,curr.Room.RoomNumber,curr.Room.Type,curr.Room.Floor,curr.TotalPrice);
                curr = curr.Next;
            }
        }
        private void filterBookings()
        {
            Con.Open();
            string Query = "select * from BookingTbl where RType = '"+RTypeCb.SelectedItem.ToString()+"'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookingDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            showBookings();
        }

        private void RTypeCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            filterBookings();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
           Program. objcustomer.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
          Program.  objuser.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           Program. objdash.Show();
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

        private void BookingDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
          nametb.Text = BookingDGV.SelectedRows[0].Cells[0].Value.ToString(); 
         Roomnumcb.Text = BookingDGV.SelectedRows[0].Cells[6].Value.ToString(); 
          RTypeCb.Text = BookingDGV.SelectedRows[0].Cells[7].Value.ToString(); 
        }

        private void bookdelbtn_Click(object sender, EventArgs e)
        {
            if (nametb.Text == "" || Roomnumcb.Text == "")
                MessageBox.Show("Please select a booking to Delete!");
            else
            {
                Program.list.DeleteBooking(int.Parse(Roomnumcb.Text),nametb.Text);
            }
        }
    }
}
