using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebAPIAddresses.Identity.Roles)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
    }
}
