using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Test.Persons
{
    public class PersonsClient : BaseClient, IPersonService
    {
        public PersonsClient(HttpClient Client) :
            base(Client, "api/person")
        {

        }

        public async Task IPersonService.Add(Person person)
        {
            
        }

        Task<int> IPersonService.Count()
        {
            throw new NotImplementedException();
        }

        Task<bool> IPersonService.Delete(int Id)
        {
            throw new NotImplementedException();
        }

        Task<Person?> IPersonService.GetById(int Id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Person>> IPersonService.GetPersons()
        {
            throw new NotImplementedException();
        }
    }
}
