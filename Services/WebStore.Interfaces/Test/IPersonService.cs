using WebStore.Domain.Entities;

namespace WebStore.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPersons();

        Task<int> Count();

        Task<Person?> GetById(int Id);

        Task<bool> Delete(int Id);

        Task Add(Person person);
    }
}
