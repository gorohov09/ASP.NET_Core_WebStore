﻿using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        private readonly ILogger<SqlEmployeesData> _Logger;

        public SqlEmployeesData(WebStoreDB db, ILogger<SqlEmployeesData> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public int Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentException(nameof(employee));

            if (_db.Employees.Contains(employee))
                return employee.Id;

            _db.Employees.Add(employee);

            _db.SaveChanges(); //только здесь Объект(employee) получит свойство Id

            return employee.Id;
        }

        public bool Delete(int id)
        {
            //var employee = GetById(id);
            var employee = _db.Employees.Select(e => new Employee()
            {
                Id = e.Id
            }).FirstOrDefault(e => e.Id == id); //Экономия памяти(Удаляем по Id)

            if (employee is null)
            {
                _Logger.LogWarning("Попытка удаления отсутсвующего сотрудника с Id:{0}", employee.Id);
                return false;
            }

            _db.Employees.Remove(employee);

            _db.SaveChanges();

            _Logger.LogInformation("Сотруднике с id:{0} был успешно удален", employee.Id);

            return true;
        }

        public bool Edit(Employee employee)
        {
            #region 1_вариант
            //if (employee == null)
            //    throw new ArgumentException(nameof(employee));

            ////if (_db.Employees.Contains(employee))
            ////    return true;

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

            //_db.SaveChanges();

            //_Logger.LogInformation("Информация о сотруднике id:{0} была изменена", employee.Id);

            //return true;
            #endregion

            _db.Employees.Update(employee);

            return _db.SaveChanges() != 0;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees;
        }

        public Employee? GetById(int id)
        {
            //return _db.Employees.FirstOrDefault(emp => emp.Id == id);
            return _db.Employees.Find(id); //Берем из кэша
        }
    }
}
