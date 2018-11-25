using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WebApplication15
{ 
    class DatabaseConnection
    {
           private static DatabaseConnection instance;
        public String ConnectionString = "Data Source=MAMONA\\MAMONA;Initial Catalog=LibraryManagementSystem;User ID=sa;Password=Mamona123;MultipleActiveResultSets=True;Application Name=EntityFramework";
        private SqlConnection connection;


        public static DatabaseConnection getInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseConnection();
            }
            return instance;
        }

        private DatabaseConnection()
        {

        }

        public SqlConnection getConnection()
        {
            connection = new SqlConnection(ConnectionString);
           if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            return connection;
        }

        public SqlDataReader getData(String commnadText)
        {
            connection = getConnection();
            SqlCommand cmd = new SqlCommand(commnadText,connection);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public int exectuteQuery(String commnadText)
        {
            connection = getConnection();
            SqlCommand cmd = new SqlCommand(commnadText,connection);
            int rows = cmd.ExecuteNonQuery();
            return rows;
        }

        public void closeConnection()
        {
            if (connection != null)
                connection.Close();
        }
    }
}
