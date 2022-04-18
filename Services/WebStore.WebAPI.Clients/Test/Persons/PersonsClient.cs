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
    public class PersonsClient : BaseClient, IPersonsService
    {
        public PersonsClient(HttpClient Client) :
            base(Client, "api/person")
        {

        }

        public async Task Add(Person person)
        {
            var response = await Http.PostAsJsonAsync(Address, person);
            response.EnsureSuccessStatusCode();
        }

        public async Task<int> Count()
        {
            var response = await Http.GetAsync($"{Address}/count");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<int>();

            return -1;
        }

        public async Task<bool> Delete(int Id)
        {
            var response = await Http.DeleteAsync($"{Address}/{Id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Person?> GetById(int Id)
        {
            var response = await Http.GetAsync($"{Address}/{Id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Person>();

            return null;
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            var response = await Http.GetAsync(Address);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<Person>>();

            return null;
        }
    }
}
