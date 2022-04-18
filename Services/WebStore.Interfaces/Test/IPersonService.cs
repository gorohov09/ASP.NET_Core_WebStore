using WebStore.Domain.Entities;

namespace WebStore.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<Person> GetPersons();

        int Count();

        Person? GetById(int Id);

        bool Delete(int Id);

        void Add(Person person);
    }
}
