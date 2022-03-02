﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient client)
            : base(client, "api/values")
        {

        }

        public void Add(string Value)
        {
            var response = Http.PostAsJsonAsync(Address, Value).Result;
            response.EnsureSuccessStatusCode();
        }

        public int Count()
        {
            var response = Http.GetAsync($"{Address}/count").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<int>().Result;

            return -1;
        }

        public bool Delete(int Id)
        {
            var response = Http.DeleteAsync(Address).Result;

            return response.IsSuccessStatusCode;
        }

        public void Edit(int Id, string Value)
        {
            var response = Http.PutAsJsonAsync($"{Address}/{Id}", Value).Result;
            response.EnsureSuccessStatusCode();
        }

        public string? GetById(int Id)
        {
            var response = Http.GetAsync($"{Address}/{Id}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;

            return null;
        }

        public IEnumerable<string> GetValues()
        {
            var response = Http.GetAsync(Address).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result!;

            return Enumerable.Empty<string>();
        }
    }
}