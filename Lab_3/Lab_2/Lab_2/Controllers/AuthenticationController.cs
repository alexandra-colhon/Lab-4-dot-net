using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Lab_2.Models;
using Lab_2.Data;
using Lab_2.ViewModel.Authentication;

namespace Lab_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser(RegisterInput registerInput)
        {
            var user = new ApplicationUser
            {
                Email = registerInput.Email,
                UserName = registerInput.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerInput.Password);
            if (result.Succeeded)
            {
                return Ok(new RegisterViewModel
                {
                    ConfirmationToken = user.SecurityStamp
                });
            }
            return BadRequest(result.Errors);
        }


        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmUser(ConfirmUserInput confirmUserInput)
        {
            var toConfirm = _context.ApplicationUsers.Where
                (u => u.Email == confirmUserInput.Email && u.SecurityStamp == confirmUserInput.ConfirmationToken)
                .FirstOrDefault();
            if (toConfirm != null)
            {
                toConfirm.EmailConfirmed = true;
                _context.Entry(toConfirm).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginInput loginInput)
        {
            var user = await _userManager.FindByEmailAsync(loginInput.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginInput.Password))
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                    //new Claim(JwtRegisteredClaimNames.na)
                };
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                  claims: claims
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo
                  });
            }

            return Unauthorized();

        }



    }

}
