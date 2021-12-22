using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities.Base.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface INameEntity : IEntity
    {
        string Name { get; set; }
    }

    public interface IOrderedEntity : IEntity
    {
        int Order { get; set; }
    }
}
