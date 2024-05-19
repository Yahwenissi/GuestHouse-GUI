using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestHouse_GUI
{
    internal static class Program
    {
        public static Customers objcustomer;
        public static Bookings objbooking;
        public static Users objuser;
        public static DashBoard objdash;
        public static Login objlogin ;
          public static Guest[] guest = new Guest[20];
        public static LinkedList list= new LinkedList();
        


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create instances of forms after setting compatible text rendering
            objdash = new DashBoard();
            objbooking = new Bookings();
            objcustomer = new Customers();
            objuser = new Users();
            objlogin = new Login();
            Application.Run(objdash);
        }
    }

    public static class Formspool
    { 
   
        public static void forms()
        {
           
        }
    }

}
