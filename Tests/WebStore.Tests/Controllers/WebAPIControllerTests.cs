using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPIControllerTests
    {
        [TestMethod]
        public void Index_returns_with_DataValues()
        {
            #region Как писать тесты к асинхронным методам?
            //var data = new List<Person>()
            //{
            //    new Person { Id = 1, LastName = "Горохов", FirstName = "Андрей", Age = 19, Birthday = new DateTime(2002, 7, 9), Salary = 100000},
            //    new Person { Id = 2, LastName = "Курочкин", FirstName = "Владислав", Age = 18, Birthday = new DateTime(2002, 6, 19), Salary = 90000},
            //    new Person { Id = 3, LastName = "Ардинцев", FirstName = "Максим", Age = 30, Birthday = new DateTime(1996, 12, 7), Salary = 20000}
            //};

            //Task<IEnumerable<Person>> task_data = new Task<IEnumerable<Person>>(() => data);

            //var persons_service_mock = new Mock<IPersonsService>();
            //persons_service_mock.Setup(c => c.GetPersons())
            //    .Returns(task_data);


            //var values_service_mock = new Mock<IValueService>();

            //var controller = new WebAPIController(values_service_mock.Object, persons_service_mock.Object);

            //var result = controller.Index();

            //var view_result = Assert.IsType<Task<IActionResult>>(result);

            //var model = Assert.IsAssignableFrom<IEnumerable<Person>>(view_result.);

            //foreach (var person in model)
            //    Assert.IsType<Person>(person);

            //var value_service_mock = new Mock<IValueService>();
            //var person_service_mock = new Mock<IPersonsService>();

            //var controller = new WebAPIController(value_service_mock.Object, person_service_mock.Object);

            //var result 
            #endregion

            var data = Enumerable.Range(1, 10)
                .Select(i => $"Value - {i}")
                .ToArray();

            //(Условный поставщик данных)
            var values_service_mock = new Mock<IValueService>(); 
            //stab - источник данных(подменяет некий интерфейс)
            //mock - другой режим

            values_service_mock
                .Setup(c => c.GetValues()) //При вызове метода GetValues
                .Returns(data); //Он будет возвращать data
            var persons_service_mock = new Mock<IPersonsService>();

            var controller = new WebAPIController(values_service_mock.Object, persons_service_mock.Object); //Создаем контроллер

            var result = controller.Index(); //На контроллере вызываем метод Index

            var view_result = Assert.IsType<ViewResult>(result.Result); //Метод проверяет соответствие типов и приводит результат к данному типу(ViewResult)

            //Говорим, что модель должна быть приводима к интерфейсу - IEnumerable<string>
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            var i = 0;
            foreach (var actual_value in model)
            {
                var expected_value = data[i++];
                Assert.Equal(expected_value, actual_value);
            }

            values_service_mock.Verify(s => s.GetValues()); //Был ли вызван метод GetValues() ?
            values_service_mock.VerifyNoOtherCalls(); //Если еще что-то дополнительно вызвано - получаем ошибку
        }
    }
}
