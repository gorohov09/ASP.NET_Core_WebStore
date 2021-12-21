using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Diagnostics;

namespace WebStore.Infrastructure.Conventions
{
    public class TestConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            //Debug.WriteLine(controller.ControllerName);

            //controller.Actions. //Добавление действий в контроллер
            //controller.RouteValues. //Изменяем параметры маршрута у контроллеров
        }
    }
}
