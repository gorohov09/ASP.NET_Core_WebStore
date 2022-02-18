using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public class NamedEntity : Entity, INamedEntity
    {
        [Required] //Свойство обязательно
        public string Name { get; set; }
    }
}
