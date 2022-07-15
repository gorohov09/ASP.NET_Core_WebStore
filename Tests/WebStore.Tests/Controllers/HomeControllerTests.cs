using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void Index_Returns_View()
    {
        var product_data_mock = new Mock<IProductData>(); //Создали мок
        product_data_mock.Setup(s => s.GetProducts(It.IsAny<ProductFilter>()))
            .Returns(Enumerable.Empty<Product>());

        IProductData product_data = product_data_mock.Object; //Получили реализацию мока

        var controller = new HomeController();

        var result = controller.Index(product_data);

        Assert.IsType<ViewResult>(result);
    }

    [TestMethod]
    public void ConfiguredAction_Returns_string_value()
    {
        //A-A-A = Arrange - Act - Assert
        #region Arrange(Исходные данные)
        const string id = "123";
        const string value_1 = "QWE";
        const string expected_string = $"Hello World! {id} - {value_1}"; //Ожидаемый результат

        var controller = new HomeController(); //Тестируемый объект
        #endregion

        #region Act(Действие, которое призвано что-то тестировать)
        var actual_string = controller.ConfiguredAction(id, value_1); //Действие, которое выполняем в рамках тестирования
        #endregion

        #region Assert(Результат)
        Assert.Equal(expected_string, actual_string);
        #endregion
    }

    [TestMethod, ExpectedException(typeof(ApplicationException))]
    public void Throw_thrown_ApplicationException()
    {
        const string exception_message = "Message";

        var controller = new HomeController();

        controller.Throw(exception_message);
    }

    [TestMethod]
    public void Throw_thrown_ApplicationException_with_Message()
    {
        const string exception_message = "Message";

        var controller = new HomeController();

        Exception? exception = null;
        try
        {
            controller.Throw(exception_message);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        var application_exception = Assert.IsType<ApplicationException>(exception);
        Assert.Equal(exception_message, application_exception.Message);
    }

    [TestMethod]
    public void Throw_thrown_ApplicationException_with_Message2()
    {
        const string exception_message = "Message";

        var controller = new HomeController();

        var actual_exception = Assert.Throws<ApplicationException>(() => controller.Throw(exception_message));

        Assert.IsType<ApplicationException>(actual_exception);
        Assert.Equal(exception_message, actual_exception.Message);
    }

    [TestMethod]
    public void Status_with_id_404_Returns_RedirectToAction_Error404()
    {
        const string status = "404";

        var controller = new HomeController();

        var result = controller.Status(status);

        Assert.NotNull(result);
    }
}
