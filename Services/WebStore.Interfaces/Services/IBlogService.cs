using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogs();
    }
}
