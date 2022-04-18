using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<Person> GetPersons();

        int Count();

        Person? GetById(int Id);

        void Add(Person person);

        bool Delete(int Id);
    }
}
