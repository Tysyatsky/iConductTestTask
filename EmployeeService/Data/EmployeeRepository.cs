using EmployeeService.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Employee> LoadEmployeesById(int id)
    {
        string sql = $@"
            WITH EmployeeTree AS
            (
                SELECT Id, Name, ManagerId, Enable
                FROM Employee
                WHERE Id = @Id
                    AND Enable = 1

                UNION ALL

                SELECT root.Id, root.Name, root.ManagerId, root.Enable
                FROM Employee root
                INNER JOIN EmployeeTree et ON root.ManagerId = et.Id
                WHERE root.Enable = 1
            )
            SELECT Id, Name, ManagerId, Enable FROM EmployeeTree;";

        var result = new List<Employee>();

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var employee = new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        ManagerId = GetNullableInt(reader, "ManagerId")
                    };

                    result.Add(employee);
                }
            }
        }

        return result;
    }

    public int UpdateEmployeeEnabled(int id, bool enabled)
    {
        const string query = @"
            UPDATE dbo.Employee
            SET Enable = @Enable
            WHERE Id = @EmployeeId";

        using (var connection = new SqlConnection(_connectionString))
        using (var sqlCommand = new SqlCommand(query, connection))
        {
            sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
            sqlCommand.Parameters.AddWithValue("@Enable", enabled);

            connection.Open();

            return sqlCommand.ExecuteNonQuery();
        }
    }

    private static int? GetNullableInt(SqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
    }
}