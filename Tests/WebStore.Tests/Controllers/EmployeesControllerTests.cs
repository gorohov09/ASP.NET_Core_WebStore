using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<EmployeesController>> _loggerMock;
        private Mock<IEmployeesData> _employeesDataMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
            _employeesDataMock = new Mock<IEmployeesData>();
        }

        [TestMethod]
        public void IndexGetListOfEmployeesCorrect()
        {
            //Arrange
            _employeesDataMock
                .Setup(e => e.GetAll()).Returns(GetEmployeesTest());

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(viewResult.Model);
            Assert.Equal(GetEmployeesTest().Count, model.Count());

            _employeesDataMock.Verify(e => e.GetAll());
            _employeesDataMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DetailsGetEmployeeCorrect()
        {
            const int expected_id = 1;
            const string expected_name = "Андрей";

            _employeesDataMock
                .Setup(e => e.GetById(It.Is<int>(id => id > 0)))
                .Returns<int>(id => new Employee
                {
                    Id = 1, 
                    FirstName = "Андрей"
                });

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Details(expected_id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.Model);
            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.FirstName);

            _employeesDataMock.Verify(e => e.GetById(It.Is<int>(id => id > 0)));
            _employeesDataMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DetailsGetEmployeeInCorrect()
        {
            var expected_id = 123;

            _employeesDataMock
                .Setup(e => e.GetById(It.Is<int>(id => id > 0)))
                .Returns<int>(null);

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Details(expected_id);

            var viewResult = Assert.IsType<NotFoundResult>(result);

            _employeesDataMock.Verify(e => e.GetById(It.Is<int>(id => id > 0)));
            _employeesDataMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void CreateEmployeeCorrect()
        {
            var expected_view_name = "Edit";

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(viewResult.ViewName, expected_view_name);
            Assert.IsType<EmployeeViewModel>(viewResult.Model);
        }

        [TestMethod]
        public void EditEmployeeCorrect()
        {
            const int expected_id = 1;
            const string expected_name = "Андрей";
            _mapperMock
                .Setup(m => m.Map<EmployeeViewModel>(It.IsAny<Employee>()))
                .Returns(new EmployeeViewModel
                {
                    Id = expected_id,
                    FirstName = expected_name,
                });

            _employeesDataMock
                .Setup(e => e.GetById(It.Is<int>(id => id > 0)))
                .Returns<int>(id => new Employee
                {
                    Id = expected_id,
                    FirstName = expected_name
                });

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);
            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.FirstName);

            _employeesDataMock.Verify(e => e.GetById(It.Is<int>(id => id > 0)));
            _employeesDataMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void EditEmployeeIdIsNull()
        {
            int? expected_id = null;

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);

            Assert.Null(model.FirstName);
            Assert.Null(model.LastName);
            Assert.Null(model.Patronymic);
            Assert.True(model.Salary == default(int));
            Assert.True(model.Birthday == default(DateTime));
            Assert.True(model.Age == default(int));
        }

        [TestMethod]
        public void EditEmployeeIsNull()
        {
            const int expected_id = 123;

            _employeesDataMock
                .Setup(e => e.GetById(It.Is<int>(id => id > 0)))
                .Returns<int>(null);

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);
            var result = controller.Edit(expected_id);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void PostEditEmployeeIncorrect()
        {
            const string expected_name = "Бин";
            const string expected_lastname = "Асама";
            const string expected_patronymic = "Ладен";

            var expected_model = new EmployeeViewModel { Id = 1, FirstName = expected_name, LastName = expected_lastname, Patronymic = expected_patronymic };

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_model);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);

            Assert.Equal(expected_name, model.FirstName);
            Assert.Equal(expected_lastname, model.LastName);
            Assert.Equal(expected_patronymic, model.Patronymic);
        }

        [TestMethod]
        public void PostEditEmployeeAdd()
        {
            var expected_model = new EmployeeViewModel { Id = 0, FirstName = "Андрей"};

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_model);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Index), viewResult.ActionName);
        }

        [TestMethod]
        public void PostEditEmployeeEditFalse()
        {
            var expected_model = new EmployeeViewModel { Id = 3, FirstName = "Андрей" };

            _employeesDataMock.Setup(e => e.Edit(It.IsAny<Employee>()))
                .Returns(false);

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_model);
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void PostEditEmployeeEditTrue()
        {
            var expected_model = new EmployeeViewModel { Id = 3, FirstName = "Андрей" };

            _employeesDataMock.Setup(e => e.Edit(It.IsAny<Employee>()))
                .Returns(true);

            var controller = new EmployeesController(_employeesDataMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = controller.Edit(expected_model);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Index), viewResult.ActionName);
        }


        private List<Employee> GetEmployeesTest() =>
            new List<Employee>()
            {
                new Employee() { Id = 1, FirstName = "Андрей"},
                new Employee() { Id = 2, FirstName = "Максим"},
                new Employee() { Id = 1, FirstName = "Дамир"},
            };
    }
}
