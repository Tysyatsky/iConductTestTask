using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string createTableQuery = @"
            CREATE TABLE Employee
            (
                Id INT PRIMARY KEY IDENTITY(1,1),
                UserName NVARCHAR(100) NOT NULL,
                Email NVARCHAR(200),
                CreatedDate DATETIME DEFAULT GETDATE()
            )";


            using (SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"))
            {
                SqlCommand command = new SqlCommand(createTableQuery, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }


            DataTable dtEmployees = GetQueryResult(createTableQuery);
        }
        
        private static DataTable GetQueryResult(string query)
        {
            var dt = new DataTable();

			using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
					command.CommandText = query;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

			return dt;
        }
    }
}
