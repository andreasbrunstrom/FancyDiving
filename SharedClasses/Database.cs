using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SharedClasses
{
    public class Database
    {
        private MySqlConnection connection;
        private string server;
        private string port;
        private string database;
        private string uid;
        private string password;
        private bool connected;

        public Database()
        {
            initialize();
        }

        private void initialize()
        {
            server = "mydb23.surf-town.net";
            port = "3306";
            database = "aspenas_simhopp";
            uid = "aspenas_simhopp";
            password = "simhopp1234";
            var connectionString = "SERVER=" + server + ";" + "PORT=" + port + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "DATABASE=" + database + ";Convert Zero Datetime=True;Allow Zero Datetime=True";

            connection = new MySqlConnection(connectionString);
        }

        public bool openConnection()
        {
            if (connected) return true;
            try
            {
                connection.Open();
                connected = true;
                return true;
            }
            catch (MySqlException ex)
            {

                switch (ex.Number)
                {

                    case 0:
                      //  System.Windows.MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1042:
                      //  System.Windows.MessageBox.Show("Can't get hostname address");
                        break;
                    case 1045:
                       // System.Windows.MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default: break;

                }
                return false;
            }
        }
        public int set(string query)
        {
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                var lastId = (int)cmd.LastInsertedId;
                closeConnection();
                return lastId;
            }
            return -1;
        }

        public List<List<string>> get(string query)
        {
            var list = new List<List<string>>();

            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    var l = new List<string>();
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        var res = dataReader.GetString(i);
                        l.Add(res);
                    }
                    list.Add(l);
                }

                dataReader.Close();
                closeConnection();
                return list;
            }
            return list;
        }
        public bool closeConnection()
        {
            if (!connected) return false;
            try
            {
                connection.Close();
                connected = false;
                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
                return false;
            }
        }
    }
}
