using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Employees;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient Client)
            : base(Client, "api/employees")
        { 
        }

        public int Add(Employee employee)
        {
            var response = Post(Address, employee.ToDTO());
            var added_employee = response?.Content.ReadFromJsonAsync<EmployeeDTO>().Result;

            if (added_employee is null)
                return -1;

            var id = added_employee.Id;

            employee.Id = id;

            return id;
        }

        public bool Delete(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return success;
        }

        public bool Edit(Employee employee)
        {
            var response = Put(Address, employee.ToDTO());

            var success = response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<bool>()
                .Result;

            return success;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employess = Get<IEnumerable<EmployeeDTO>>(Address);
            return employess.FromDTO()!;
        }

        public Employee? GetById(int id)
        {
            var employee = Get<EmployeeDTO>($"{Address}/{id}");
            return employee.FromDTO();
        }
    }
}
