using System;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
//using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;








public class Guest
    {
        public string FullName, PhoneNumber, CheckInDate, CheckOutDate, gender, Dob, status;
        public int? Age;

        public Guest(string fullName, string phoneNumber, string checkInDate, string checkOutDate, int? age, string dob, string gender)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            Age = age;
            this.gender = gender;
            Dob = dob;
        }
        public Guest(string fullName, string phoneNumber, string dob, int? age,string gender)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Dob = dob;
            Age=age;
            this.gender = gender;
            status = "pending";

        }
        public Guest() { }
    }


    public class Room
    {
        public int RoomNumber, Floor;
        public string Type, Status;
        public double Price;
        public Room() { }
        public Room(int roomNumber, int floor, string type, string status, double price)
        {
            RoomNumber = roomNumber;
            Floor = floor;
            Type = type;
            Status = status;
            Price = price;
        }
    }

    public class Booking
    {
        public Room Room;

        public Guest Guest;

        public string BookingDate;
        public double? TotalPrice;
        public Booking Next, Prev;

        public Booking(Room room, Guest guest, string bookDate, double? totalPrice)
        {
            Next = Prev = null;
            if (guest != null)
            {
                Guest = guest;
            }
            Room = room;
            BookingDate = bookDate;
            TotalPrice = totalPrice;
        }
    }

    public class Database
    {

        public Database() { }
        string connectionString = "Data Source = RAFA; Initial Catalog = GuestHouse; Integrated Security = True";
        SqlConnection connection;
        SqlCommand command;

        public void FetchTolist(LinkedList list)
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand("Select * from BookingView;", connection);

            SqlDataReader reader;
            try
            {
                using (connection)
                {

                    connection.Open();

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Room room = new Room();


                        room.RoomNumber = reader.GetInt32(reader.GetOrdinal("Room Number"));
                        room.Floor = reader.GetInt32(reader.GetOrdinal("Floor"));
                        room.Type = reader.GetString(reader.GetOrdinal("Room Type"));
                        room.Status = "occupied";
                        room.Price = (double)reader.GetDecimal(reader.GetOrdinal("Room Price"));

                        Guest guest = new Guest();
                        guest.FullName = reader.GetString(reader.GetOrdinal("Full Name"));
                        guest.PhoneNumber = reader.GetString(reader.GetOrdinal("Phone Number"));
                        guest.CheckInDate = reader.GetDateTime(reader.GetOrdinal("Check In Date")).ToString("yyyy-MM-dd ");
                        guest.CheckOutDate = reader.GetDateTime(reader.GetOrdinal("Check Out Date")).ToString("yyyy-MM-dd ");
                        guest.gender = reader.GetString(reader.GetOrdinal("Gender"));
                        guest.Age = reader.GetInt32(reader.GetOrdinal("Age"));
                    guest.Dob = reader.GetDateTime(reader.GetOrdinal("Date Of Birth")).ToString("yyyy-MM-dd");

                        string bookdate = reader.GetDateTime(reader.GetOrdinal("Book Date")).ToString("yyyy-MM-dd ");
                        double totalprice = (double)reader.GetDecimal(reader.GetOrdinal("Total Price"));
                        list.AddTolist(room, guest, bookdate, totalprice);
                    }
                }
                connection.Close();

                using (connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    command = new SqlCommand("Select * from RoomsView;", connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Room room = new Room();
                        room.Floor = reader.GetInt32(reader.GetOrdinal("Floor"));
                        room.RoomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"));
                        room.Type = reader.GetString(reader.GetOrdinal("RoomType"));
                        room.Status = reader.GetString(reader.GetOrdinal("Status"));
                        room.Price = (double)reader.GetDecimal(reader.GetOrdinal("RoomPrice"));
                        list.AddTolist(room);
                    }
                }

            }

            finally
            {
                connection.Close();
            }


        }

        public void AddFromlist(Booking book)
        {
            if (book == null || book.Guest == null || book.Room == null)
            {
                // Handle the case where book or its properties are null
              MessageBox.Show("Invalid booking information.");
                return;
            }

            connection = new SqlConnection(connectionString);
            try
            {
                using (command = new SqlCommand("InsertintoBookings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@Gname", book.Guest.FullName);
                    command.Parameters.AddWithValue("@GDOB", book.Guest.Dob);
                    command.Parameters.AddWithValue("@Ggender", book.Guest.gender);
                    command.Parameters.AddWithValue("@Gphone", book.Guest.PhoneNumber);
                    command.Parameters.AddWithValue("@Gcheckindate", book.Guest.CheckInDate);
                    command.Parameters.AddWithValue("@Gcheckoutdate", book.Guest.CheckOutDate);
                    command.Parameters.AddWithValue("@roomnum", book.Room.RoomNumber);

                    connection.Open();
                   int rowsaff= command.ExecuteNonQuery();
                if(rowsaff > 0)
                {
                    MessageBox.Show("Booked Successfully!");
                }
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public void ModifyGuest(Booking book)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (command = new SqlCommand("MODIFYBOOKING", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@name", book.Guest.FullName);
                    command.Parameters.AddWithValue("@DOB", DateTime.Parse(book.Guest.Dob));
                    command.Parameters.AddWithValue("@phone", book.Guest.PhoneNumber);
/*                    command.Parameters.AddWithValue("@Gcheckindate", book.Guest.CheckInDate);
                    command.Parameters.AddWithValue("@Gcheckoutdate", book.Guest.CheckOutDate);*/
                    command.Parameters.AddWithValue("@roomnum", book.Room.RoomNumber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public void DeleteBooking(int roomnum)
        {

            connection = new SqlConnection(connectionString);
            try
            {
                using (SqlCommand command = new SqlCommand("delete from Booking Where RoomNumber= @roomnum", connection))
                {
                    // Add parameter
                    command.Parameters.AddWithValue("@roomnum", roomnum);
                    connection.Open();
                 int rowaff=   command.ExecuteNonQuery();
                if (rowaff > 0)
                    MessageBox.Show("Booking Deleted!");
                }
            }
            finally { connection.Close(); }
        }

        public void DeleteAllBookings()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (SqlCommand command = new SqlCommand("delete from Booking; Update Guests set Status='checked out';", connection))
                {


                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();


                }
                using (SqlCommand command = new SqlCommand("update Rooms set Status='available'", connection))
                {
                    /*                connection = new SqlConnection(connectionString);
                    */
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            finally { connection.Close(); }
        }
    }

    public class LinkedList
    {
        private Booking head, tail;
       
    public Booking Head { get { return head; } }

        private int totalBooking = 0, totalRoom = 0;
        public int TotalBooking { get { return totalBooking; } set { TotalBooking = value; } }
        public int TotalRoom { get { return totalRoom; } set { totalRoom = value; } }

        public Database Db = new Database();

        public LinkedList()
        {
            Db.FetchTolist(this);

        }

        public void AddTolist(Room room, Guest guest = null, string bookDate = null, double? totalPrice = null)
        {
            Booking newBooking = new Booking(room, guest, bookDate, totalPrice);

            if (head == null)
            {
                head = newBooking;

                tail = head;
            }
            else
            {
                tail.Next = newBooking;
                newBooking.Prev = tail;
                tail = newBooking;
            }
            if (guest != null)
                totalBooking++;



            totalRoom++;
        }

        public void AddBooking(int roomNumber, Guest guest, double totalPrice)
        {
            Booking current = head;
            bool roomFound = false;


            while (current != null)
            {
                if (current.Room.RoomNumber == roomNumber && current.Guest == null)
                {
                    current.Guest = guest;
                    current.Room.Status = "occupied";
                    current.BookingDate = guest.CheckInDate;
                    current.TotalPrice = totalPrice;
                    roomFound = true;
                    break;
                }
                current = current.Next;
            }

            Db.AddFromlist(current);

            if (!roomFound)
            {
                Console.WriteLine("Room is occupied!!");
            }
        }
        public void DisplayBooking(Booking booking)
        {
            try
            {

                Console.WriteLine("| Room Number | Floor | Full Name       | Check-in Date  | Check-out Date  | Total Price |");
                Console.WriteLine("|-------------|-------|-----------------|----------------|-----------------|-------------|");
                Console.WriteLine($"| {booking.Room.RoomNumber,-12}| {booking.Room.Floor,-5} | {booking.Guest.FullName,-11}    | {booking.Guest.CheckInDate,-14} | {booking.Guest.CheckOutDate,-15} | {booking.TotalPrice}        |");
                Console.WriteLine("|-------------|-------|-----------------|----------------|-----------------|-------------|");

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }


        }


        public void DisplaySingleBooking(string name = null, int? roomNumber = null, int? floor = null)
        {
            Booking current = head;
            bool found = false;
            try
            {
                Console.WriteLine("==========================================================================================");
                Console.WriteLine("                                     Booking Information                                  ");
                Console.WriteLine("==========================================================================================");
                while (current != null)
                {
                    if (current.Guest != null)
                    {
                        bool matchName = string.IsNullOrEmpty(name) || current.Guest.FullName == name;
                        bool matchRoomNumber = roomNumber == null || roomNumber == -1 || current.Room.RoomNumber == roomNumber;
                        bool matchFloor = floor == null || floor == -1 || current.Room.Floor == floor;

                        if (matchName && matchRoomNumber && matchFloor)
                        {

                            DisplayBooking(current);
                            Console.WriteLine("==========================================================================================");

                            found = true;
                        }
                    }
                    current = current.Next;
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Booking Not Found!");
                return;
            }

            if (!found)
            {
                Console.WriteLine("Booking not found.");
            }
        }


        public void DisplayAllBookings()
        {
            Booking current = head;

            if (current == null)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            Console.WriteLine("All Bookings:");
            Console.WriteLine("==========================================================================================");
            Console.WriteLine("                                     Booking Information                                  ");
            Console.WriteLine("==========================================================================================");
            while (current != null)
            {
                if (current.Room.Status == "occupied")
                {
                    DisplayBooking(current);
                    Console.WriteLine("==========================================================================================");


                }
                current = current.Next;
            }
        }
        public void DeleteBooking(int? roomnum = null, string name = null)
        {
        
        DialogResult result = MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
           if (result== DialogResult.No)
                return;


            Booking current = head;
            bool found = false;

            while (current != null)
            {
                if (current.Guest != null)
                {
                    if ((roomnum != null && current.Room.RoomNumber == roomnum) ||
                        (roomnum == null && !string.IsNullOrEmpty(name) && current.Guest.FullName.ToLower() == name.ToLower()))
                    {
                        // Set Guest property to null
                        current.Guest = null;
                        Db.DeleteBooking(current.Room.RoomNumber);

                        // Update status to available
                        current.Room.Status = "available";

                        found = true;
                        totalBooking--;
                        break;
                    }
                }
                current = current.Next;
            }

            if (!found)
            {
                Console.WriteLine("Booking not found.");
            }
            else
            {
                Console.WriteLine("Booking deleted successfully.");
            }
        }

    public void DeleteAllBookings()
    {
        Booking current = head;

        DialogResult result = MessageBox.Show("Are you sure you want to delete all bookings?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            while (current != null)
            {
                current.Guest = null; 
                current.Room.Status = "available"; 
                current = current.Next; 
            }

            totalBooking = 0; 
            Db.DeleteAllBookings(); 
            MessageBox.Show("All bookings deleted successfully.", "Deletion Complete", MessageBoxButtons.OK, MessageBoxIcon.Information); // Confirm deletion
        }
        else
        {
            // If user did not confirm, return without deleting anything
            MessageBox.Show("Deletion of all bookings was cancelled.", "Deletion Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information); // Inform user
            return;
        }
    }
    public void DisplayAvailable(int? roomnum = null)
        {
            Booking curr = head;

            if (roomnum == null)
                while (curr != null)
                {
                    if (curr.Room.Status == "available")
                    {
                        Console.WriteLine($"| {curr.Room.RoomNumber,-12} | {curr.Room.Floor,-5} | {curr.Room.Type,-9} | {curr.Room.Price,-5} |");
                        Console.WriteLine("|--------------|-------|-----------|-------|");

                    }
                    curr = curr.Next;
                }
            else
            {
                while (curr != null)
                {
                    if (curr.Room.Status == "available" && curr.Room.RoomNumber == roomnum)
                    {
                        Console.WriteLine($"| {curr.Room.RoomNumber,-12} | {curr.Room.Floor,-5} | {curr.Room.Type,-9} | {curr.Room.Price,-5} |");
                        Console.WriteLine("|--------------|-------|-----------|-------|");

                    }
                    curr = curr.Next;
                }
            }
            Console.WriteLine("=======================================");
        }
        public double getPrice(int roomnum)
        {
            double totalprice = 0;

            Booking current = head;

            while (current != null)
            {
                if (current.Room.RoomNumber == roomnum)
                    totalprice = current.Room.Price;

                current = current.Next;
            }



            return totalprice;
        }


        public void CheckAvailable(int? roomNumber = null, string type = null)
        {
            Booking curr = head;
            bool found = false;
            if (string.IsNullOrEmpty(type) && roomNumber != null)
                while (curr != null)
                {
                    if (curr.Room.RoomNumber == roomNumber)
                    {
                        found = true;
                        if (curr.Room.Status == "available")
                        {
                            Console.WriteLine("Room " + roomNumber + " is Available.");
                            DisplayAvailable(curr.Room.RoomNumber);
                       

                        }
                        else
                        {
                            Console.WriteLine("Room " + roomNumber + " is not Available.");
                        
                        }
                        break;
                    }
                    curr = curr.Next;
                }
            else if (!string.IsNullOrEmpty(type) && roomNumber == null)
            {
                while (curr != null)
                {
                    if (curr.Room.Type == type)
                    {
                        found = true;
                        if (curr.Room.Status == "available")
                        {
                            DisplayAvailable(curr.Room.RoomNumber);
                       

                        }


                    }
                    curr = curr.Next;
                }

            }
            if (!found)
            {
                Console.WriteLine("Room " + roomNumber + " does not exist.");
            }
        }

 

        public bool Isbooked(int roomnumber)
        {

            Booking current = head;
            while (current != null)
            {
                if (current.Room.RoomNumber == roomnumber && current.Room.Status == "available")
                    return false;

                current = current.Next;
            }
            return true;
        }

        public void ModifyGuest(Guest guest, int roomNumber , double? total = null)
        {
            Booking current = head;
            bool found = false;

            while (current != null)
            {
                if (current.Guest != null && current.Room.RoomNumber == roomNumber)
               
                {
                    // Update booking information only if parameters are not null
                    if (!string.IsNullOrEmpty(guest.FullName))
                        current.Guest.FullName = guest.FullName;
                    if (!string.IsNullOrEmpty(guest.PhoneNumber))
                        current.Guest.PhoneNumber = guest.PhoneNumber;
                    if (!string.IsNullOrEmpty(guest.Dob))
                    current.Guest.Dob = guest.Dob;
                if (guest.Age.HasValue)
                    current.Guest.Age = guest.Age;
                if(!string.IsNullOrEmpty(guest.gender))
                    current.Guest.gender=guest.gender;
           

                found = true;

                    // Update database with modified booking
                    Db.ModifyGuest(current);

                    break;
                }

                current = current.Next;
            }

            if (!found)
            {
                Console.WriteLine("Guest not found.");
            MessageBox.Show("Guest not found!");
            }
            else
            {
                Console.WriteLine("Guest updated successfully.");
            }
        }

    }
