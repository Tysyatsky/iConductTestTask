using EmployeeService.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;

namespace EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository(_connectionString);
        }
        public EmployeeNode GetEmployeeById(int id)
        {
            var employees = _employeeRepository.LoadEmployeesById(id);

            return BuildEmployeeTree(id, employees);
        }

        public void EnableEmployee(int id, int enable)
        {
            if (enable != 1 && enable != 0)
            {
                throw new WebFaultException<string>(
                    $"Integer value {id} is not valid",
                    HttpStatusCode.BadRequest);
            }

            var rowsAffected = _employeeRepository.UpdateEmployeeEnabled(id, Convert.ToBoolean(enable));

            if (rowsAffected == 0)
            {
                throw new WebFaultException<string>(
                    $"Employee with id {id} not found.",
                    HttpStatusCode.NotFound);
            }
        }

        private EmployeeNode BuildEmployeeTree(int rootId, List<Employee> employees)
        {
            var root = employees.FirstOrDefault(e => e.Id == rootId);

            if (root == null)
            {
                throw new WebFaultException<string>(
                    $"Employee with {rootId} not found",
                    HttpStatusCode.NotFound);
            }

            EmployeeNode Build(int childId)
            {
                var employee = employees.FirstOrDefault(e => e.Id == childId);
                var lookup = employees.ToLookup(e => e.ManagerId);

                return new EmployeeNode
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    ManagerId = employee.ManagerId,
                    Employees = lookup[employee.Id].Select(child => Build(child.Id)).ToList()
                };
            }

            return Build(root.Id);
        }
    }
}
