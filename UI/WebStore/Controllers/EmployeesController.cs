using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;
        private readonly IMapper _Mapper;

        public EmployeesController(IEmployeesData EmployeesData, IMapper Mapper, ILogger<EmployeesController> Logger)
        {
            _Logger = Logger;
            _EmployeesData = EmployeesData;
            _Mapper = Mapper;
        }

        public IActionResult Index()
        {
            var result = _EmployeesData.GetAll();
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.GetById(id);

            if (employee is null)
                return NotFound();
            
            return View(employee);
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }

        [HttpGet]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View(new EmployeeViewModel());
            }
            else
            {
                var employee = _EmployeesData.GetById((int)id);

                if (employee is null)
                {
                    _Logger.LogWarning("При редактировании сотрудника с id:{0} он не был найден", id);
                    return NotFound();
                }

                #region Старый вариант без AutoMapper
                //var model = new EmployeeViewModel()
                //{
                //    Id = employee.Id,
                //    LastName = employee.LastName,
                //    FirstName = employee.FirstName,
                //    Patronymic = employee.Patronymic,
                //    Age = employee.Age,
                //    Birthday = employee.Birthday,
                //    Salary = employee.Salary
                //};
                #endregion

                var model = _Mapper.Map<EmployeeViewModel>(employee);

                return View(model);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (Model.LastName == "Асама" && Model.FirstName == "Бин" && Model.Patronymic == "Ладен")
                ModelState.AddModelError("", "Террористов на работу не берем!");

            if (!ModelState.IsValid)
                return View(Model);

            #region Старый варинат без AutoMapper
            //var employee = new Employee
            //{
            //    Id = Model.Id,
            //    LastName = Model.LastName,
            //    FirstName = Model.FirstName,
            //    Patronymic = Model.Patronymic,
            //    Age = Model.Age,
            //    Birthday = Model.Birthday,
            //    Salary = Model.Salary
            //};
            #endregion

            var employee = _Mapper.Map<Employee>(Model);

            if (Model.Id == 0)
            {
                _EmployeesData.Add(employee);
                _Logger.LogInformation("Создание нового сотрудника {0}", employee);

            }

            else if (!_EmployeesData.Edit(employee))
                return NotFound();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            
            var employee = _EmployeesData.GetById(id);

            if (employee is null)
                return NotFound();

            #region Старый варинат без AutoMapper
            //var model = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    LastName = employee.LastName,
            //    FirstName = employee.FirstName,
            //    Patronymic = employee.Patronymic,
            //    Age = employee.Age
            //};
            #endregion

            var model = _Mapper.Map<EmployeeViewModel>(employee);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_EmployeesData.Delete(id))
                return NotFound();
            _Logger.LogInformation("Сотрудник с id:{0} удален", id);

            return RedirectToAction("Index");
        }
    }
}
