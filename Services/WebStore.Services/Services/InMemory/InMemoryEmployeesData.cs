using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;
        private ICollection<Employee> _Employees;
        private int _MaxFreeId;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _Logger = Logger;
            _Employees = TestData.Employees;
            _MaxFreeId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
        }

        public int Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentException(nameof(employee));

            if (_Employees.Contains(employee))
                return employee.Id;

            employee.Id = _MaxFreeId++;
            _Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee is null)
            {
                _Logger.LogWarning("Попытка удаления отсутсвующего сотрудника с Id:{0}", employee.Id);
                return false;
            }

            _Employees.Remove(employee);

            _Logger.LogInformation("Сотруднике с id:{0} был успешно удален", employee.Id);

            return true;
        }

        public bool Edit(Employee employee)
        {
            if (employee == null)
                throw new ArgumentException(nameof(employee));

            if (_Employees.Contains(employee))
                return true;

            var db_employee = GetById(employee.Id);

            if (db_employee is null)
            {
                _Logger.LogWarning("Попытка редактирования отсутсвующего сотрудника с Id:{0}", employee.Id);
                return false;
            }


            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;
            db_employee.Birthday = employee.Birthday;
            db_employee.Salary = employee.Salary;

            _Logger.LogInformation("Информация о сотруднике id:{0} была изменена", employee.Id);

            return true;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _Employees;
        }

        public Employee? GetById(int id)
        {
            return _Employees.FirstOrDefault(emp => emp.Id == id);
        }
    }
}
