using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL
{
    public class SqlBlogService : IBlogService
    {
        private readonly WebStoreDB _db;

        private readonly ILogger<SqlBlogService> _logger;

        public SqlBlogService(WebStoreDB db, ILogger<SqlBlogService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Blog>> GetBlogs() =>
            await _db.Blogs.ToListAsync().ConfigureAwait(false);
    }
}
