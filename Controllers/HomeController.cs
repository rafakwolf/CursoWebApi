using Microsoft.AspNetCore.Mvc;

namespace Aula02Api.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public string Index()
        {
            return "Api rodando :)";
        }
    }
}
