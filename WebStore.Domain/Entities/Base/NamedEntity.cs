using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public class NamedEntity : Entity, INamedEntity
    {
        public string Name { get; set; }
    }
}
