using Aula02Api.Auth;
using Aula02Api.Db;
using Aula02Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aula02Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController: Controller
    {
        private readonly DataContext _db;
        private readonly JwtProvider _tokenProvider;

        public UsersController(DataContext db, JwtProvider tokenProvider)
        {
            _db = db;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = _db.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Senha);
            if (user == null)
                return BadRequest("Usuário e/ou senha inválidos");

            var encodedToken = _tokenProvider.CreateEncoded(user.Email);
            return Json(new 
            {
                Nome = user.Name,
                Email = user.Email,
                Token = encodedToken,
                Expira = DateTime.Now
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] UserDto createAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _db.Users.FirstOrDefault(u => u.Email == createAccount.Email);
            if (user != null)
                return BadRequest("Usuário já existe");

            user = new User
            {
                Name = createAccount.Name,
                Email = createAccount.Email,
                Password = createAccount.Password
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            createAccount.Id = user.Id;

            return Created("", createAccount);
        }
    }
}
