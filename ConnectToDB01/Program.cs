using System;
using System.Data;
using System.Data.SqlClient;

namespace ConnectToDB01
{

    class Program
    {

        private static string connectionString =
            "Server=ealdb1.eal.local;Database=EJL67_DB;User ID=ejl67_usr;Password=Baz1nga67";
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {

            bool running = true;
            try
            {
                do
                {
                    int input = Menu();
                    switch (input)
                    {
                        case 1: InsertCustomer(); break;
                        //case 2: InsertPet(); break;
                        //case 3: InsertBreed(); break;
                        case 4: GetAllCustomers(); break;
                        //case 5: GetAllPets(); break;
                        //case 6: GetAllBreeds(); break;
                        case 7: SearchByLastName(); break;
                        //case 8: SearchByEmailFirstName(); break;
                        case 9: Customers(); break;
                        case 10: running = false; break;
                    }
                    Console.Clear();
                } while (running);
            }
            catch (SqlException e)
            {
                Console.WriteLine("UPS " + e.Message);
                Console.ReadKey();
            }

        }

        private int Menu()
        {

            Console.WriteLine("Commands:\n1) Insert new customer\n" + "4) Get all customers\n5) Search customers by phone number\n" +
                                            "10) End program\n");
            Console.WriteLine("Please input your command");
            string input = Console.ReadLine();
            Console.Clear();
            int x = Convert.ToInt32(input);
            return x;
        }

        private void Customers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdOwnersPets = new SqlCommand("GetInformation", con);
                cmdOwnersPets.CommandType = CommandType.StoredProcedure;
                string input = GetInput("OwnerID to search for");
                cmdOwnersPets.Parameters.Add(new SqlParameter("OwnerID", input));

                SqlDataReader reader = cmdOwnersPets.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ownerName = reader["OwnerName"].ToString();
                        string petName = reader["PetName"].ToString();
                        string petType = reader["PetType"].ToString();
                        string petBreed = reader["PetBreed"].ToString();
                        string averageLifeExpectancy = reader["AverageLifeExpectancy"].ToString();
                        Console.WriteLine(ownerName + " " + petName + " " + petType + " " + petBreed + " " + averageLifeExpectancy);
                    }
                    Console.ReadKey();
                }
            }
        }
        
        private void GetAllCustomers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdGetAllCustomers = new SqlCommand("GetCustomers", con);
                cmdGetAllCustomers.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmdGetAllCustomers.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //string id = reader["CustomerID"].ToString();
                        string CustomerLastName = reader["LastName"].ToString();
                        string CustomerFirstName = reader["FirstName"].ToString();
                        string CustomerPhoneNumber = reader["Phone"].ToString();
                        //Console.WriteLine(/*id + " " + */ CustomerLastName + " " + CustomerFirstName + " " + CustomerPhoneNumber);

                        Console.WriteLine("First name -" + " " + CustomerFirstName);
                        Console.WriteLine("Last name -" + " " + CustomerLastName);
                        Console.WriteLine("Telephone number -" + " " + CustomerPhoneNumber);
                        Console.WriteLine();
                    }
                    Console.ReadKey();
                }
            }
        }

       
       
        private void SearchByLastName()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdSearchLastName = new SqlCommand("GetOwnersByLastName", con);
                cmdSearchLastName.CommandType = CommandType.StoredProcedure;
                string input = GetInput("OwnerLastName to search for");
                cmdSearchLastName.Parameters.Add(new SqlParameter("OwnerLastName", input));

                SqlDataReader reader = cmdSearchLastName.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string id = reader["OwnerID"].ToString();
                        string firstName = reader["OwnerFirstName"].ToString();
                        string lastName = reader["OwnerLastName"].ToString();
                        string phoneNumber = reader["OwnerPhoneNumber"].ToString();
                        string email = reader["OwnerEmail"].ToString();
                        Console.WriteLine(id + " " + firstName + " " + lastName + " " + phoneNumber + " " + email);
                        Console.ReadKey();
                    }
                }
            }
        }

        private void InsertCustomer()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdOwner = new SqlCommand("InsertOwner", con);
                cmdOwner.CommandType = CommandType.StoredProcedure;
                string input = GetInput("OwnerID");
                cmdOwner.Parameters.Add(new SqlParameter("OwnerID", input));
                input = GetInput("OwnerLastName");
                cmdOwner.Parameters.Add(new SqlParameter("OwnerLastName", input));
                input = GetInput("OwnerFirstName");
                cmdOwner.Parameters.Add(new SqlParameter("OwnerFirstName", input));
                input = GetInput("OwnerPhoneNumber");
                cmdOwner.Parameters.Add(new SqlParameter("OwnerPhoneNumber", input));
                input = GetInput("OwnerEmail");
                cmdOwner.Parameters.Add(new SqlParameter("OwnerEmail", input));

                cmdOwner.ExecuteNonQuery();
            }
            Console.WriteLine("Owner Added");
        }

        private string GetInput(string info)
        {
            Console.WriteLine("Please input the " + info);
            string input = Console.ReadLine();
            return input;
        }
    }
}