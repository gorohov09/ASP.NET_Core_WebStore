using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL
{
    public class SqlUserService : IUserService
    {
        private readonly WebStoreDB _db;

        private readonly UserManager<User> _UserManager;

        public SqlUserService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<User?> GetByLogin(string login)
        {
            return _db.Users.FirstOrDefault(user => user.UserName == login);
        }
    }
}
