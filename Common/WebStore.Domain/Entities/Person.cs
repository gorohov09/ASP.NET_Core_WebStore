using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Person : Entity
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public int Age { get; set; }

        public long Salary { get; set; }

        public DateTime Birthday { get; set; }
    }
}
