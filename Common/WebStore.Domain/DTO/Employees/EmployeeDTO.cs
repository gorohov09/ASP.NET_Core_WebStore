using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Employees
{
    public static class EmployeeDTOMapper
    {
        public static EmployeeDTO? ToDTO(this Employee employee) =>
            employee is null
            ? null
            : new EmployeeDTO
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Birthday = employee.Birthday,
                Salary = employee.Salary,
            };

        public static Employee? FromDTO(this EmployeeDTO employeeDTO) =>
            employeeDTO is null
            ? null
            : new Employee
            {
                Id = employeeDTO.Id,
                LastName = employeeDTO.LastName,
                FirstName = employeeDTO.FirstName,
                Patronymic = employeeDTO.Patronymic,
                Age = employeeDTO.Age,
                Birthday = employeeDTO.Birthday,
                Salary = employeeDTO.Salary,
            };

        public static IEnumerable<EmployeeDTO?> ToDTO(this IEnumerable<Employee> employees) => employees.Select(ToDTO);

        public static IEnumerable<Employee?> FromDTO(this IEnumerable<EmployeeDTO> employees) => employees.Select(FromDTO);
    }

    public class EmployeeDTO
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public int Salary { get; set; }
    }
}
