using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController() { }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            return Ok("Returns all users"); 
        }
    }
}
