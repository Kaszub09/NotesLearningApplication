using DatabaseServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesLearningApplication.Shared.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotesLearningApplication.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        IUsersDbService _usersDbService;
        IConfiguration _configuration;
        public UsersController(IUsersDbService usersDbService, IConfiguration configuration) {
            _usersDbService = usersDbService;
            _configuration = configuration;
        }


        // POST api/<UsersController>
        [HttpPost("register")]
        public async Task<ActionResult<string>> Post(UserDTO newUser) {
            await _usersDbService.AddUserAsync(newUser);
            return Ok(CreateToken(_usersDbService.ValidateUserAsync(newUser)));
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public ActionResult<string> Login(UserDTO user) {
            return Ok(CreateToken(_usersDbService.ValidateUserAsync(user)));
        }

        private string CreateToken(UserInfoDTO user) {
            var claims = _usersDbService.GetUser(user).Claims.Select(claim => new Claim(claim.Key,claim.Value));
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("SECRET:SecurityKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(7),signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
