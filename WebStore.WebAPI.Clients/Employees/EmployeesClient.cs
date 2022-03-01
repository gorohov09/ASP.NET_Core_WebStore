﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
            var response = Post(Address, employee);
            var added_employee = response?.Content.ReadFromJsonAsync<Employee>().Result;

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
            var response = Put(Address, employee);

            var success = response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<bool>()
                .Result;

            return success;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employess = Get<IEnumerable<Employee>>(Address);
            return employess!;
        }

        public Employee? GetById(int id)
        {
            var employee = Get<Employee>($"{Address}/{id}");
            return employee;
        }
    }
}
