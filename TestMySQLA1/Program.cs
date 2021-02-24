using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data;




namespace SpikeMySQLA1
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string port;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "mydb23.surf-town.net";
            port = "3306";
            database = "aspenas_simhopp";
            uid = "aspenas_simhopp";
            password = "simhopp1234";
            var connectionString = "SERVER=" + server + ";" + "PORT=" + port +";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "DATABASE=" +
                                      database + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database. Blah
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                
                switch (ex.Number)
                {
                    
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1042:
                        MessageBox.Show("Can't get hostname address");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    defaut: break;

                }
                //Console.WriteLine(ex.Number.ToString());
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO contestants (name, nationality, gender, birthdate) VALUES('Anton Starck', 'SWE', 1, '1988-01-03')";
            
            //open connection
            if(this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE contestants SET name = 'Spaghetti Monster', nationality = 'DK', gender = 1,  birthdate = '0001-01-01' WHERE name = 'Anton Starck'";

            //Open connection
            if(this.OpenConnection() == true)
            {
                //create MySql command
                MySqlCommand cmd = new MySqlCommand();
                //assign the query using CommandText
                cmd.CommandText = query;
                //assign the connection using connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public List<string>[] Select()
        {
            string query = "SELECT * FROM contestants";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["gender"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting:");

            DBConnect db = new DBConnect();
            db.Insert();
            List<string>[] result;
            result = db.Select();
            db.Update();

            for(var i = 0; i < result[0].Count; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Console.Write(result[j][i] + " : ");
                }
                Console.Write("\n");
            }

            result = db.Select();

            for (var i = 0; i < result[0].Count; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Console.Write(result[j][i] + " : ");
                }
                Console.Write("\n");
            }
            //if (db.OpenConnection() == true)
            //{
            //    Console.WriteLine("Database Connected");

            //    db.CloseConnection();
            //}
            //else
            //    Console.WriteLine("Couldnt Connect");

            Console.ReadKey();
        }

       
        
    }
}
