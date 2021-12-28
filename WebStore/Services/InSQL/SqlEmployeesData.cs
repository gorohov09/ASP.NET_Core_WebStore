using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        public SqlEmployeesData(WebStoreDB db)
        {
            _db = db;
        }

        public int Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentException(nameof(employee));

            if (_db.Employees.Contains(employee))
                return employee.Id;

            _db.Employees.Add(employee);
            _db.SaveChanges();

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee is null)
            {
                return false;
            }

            _db.Remove(employee);
            _db.SaveChanges();

            return true;
        }

        public bool Edit(Employee employee)
        {
            //if (employee == null)
            //    throw new ArgumentException(nameof(employee));

            //if (_Employees.Contains(employee))
            //    return true;

            //var db_employee = GetById(employee.Id);

            //if (db_employee is null)
            //{
            //    _Logger.LogWarning("Попытка редактирования отсутсвующего сотрудника с Id:{0}", employee.Id);
            //    return false;
            //}


            //db_employee.FirstName = employee.FirstName;
            //db_employee.LastName = employee.LastName;
            //db_employee.Patronymic = employee.Patronymic;
            //db_employee.Age = employee.Age;
            //db_employee.Birthday = employee.Birthday;
            //db_employee.Salary = employee.Salary;

            //_Logger.LogInformation("Информация о сотруднике id:{0} была изменена", employee.Id);

            return true;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees;
        }

        public Employee? GetById(int id)
        {
            return _db.Employees.FirstOrDefault(emp => emp.Id == id);
        }
    }
}
