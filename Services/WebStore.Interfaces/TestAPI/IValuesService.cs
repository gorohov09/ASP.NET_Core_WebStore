using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces.TestAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetValues();

        string GetById(int Id);

        int Count();

        void Add(string Value);

        void Edit(int Id, string Value);

        bool Delete(int Id);
    }
}
