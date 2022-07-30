using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Blogs)]
    [ApiController]
    public class BlogsApiController : ControllerBase
    {
        private readonly IBlogService _BlogService;

        private readonly ILogger<BlogsApiController> _Logger;

        public BlogsApiController(IBlogService BlogService, ILogger<BlogsApiController> Logger)
        {
            _BlogService = BlogService;
            _Logger = Logger;   
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            _Logger.LogInformation("Начало получения блогов");
            var blogs = await _BlogService.GetBlogs();
            return Ok(blogs);
        }
    }
}
