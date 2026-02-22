using System.Collections.Generic;

namespace EmployeeService.Data
{
    internal interface IEmployeeRepository
    {
        List<Employee> LoadEmployeesById(int id);
        int UpdateEmployeeEnabled(int id, bool enabled);
    }
}
