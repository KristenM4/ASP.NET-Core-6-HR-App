using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeaWolf.HR.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CreateTokenController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<HRUser> _signInManager;
        private readonly UserManager<HRUser> _userManager;
        private readonly IConfiguration _config;

        public CreateTokenController(ILogger<AccountController> logger,
            SignInManager<HRUser> signInManager,
            UserManager<HRUser> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        /// <summary>
        /// Generate an authentication token
        /// </summary>
        /// <param name="model">A valid username and password</param>
        /// <returns>IActionResult with token string</returns>
        /// <response code="201">Token successfully generated</response>
        /// <response code="400">Invalid credentials or API error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);

                    if (user != null)
                    {
                        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                        if (result.Succeeded)
                        {
                            var claims = new[]
                            {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(
                                _config["Tokens:Issuer"],
                                _config["Tokens:Audience"],
                                claims,
                                signingCredentials: credentials,
                                expires: DateTime.UtcNow.AddMinutes(60));

                            return Created("", new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo
                            });
                        }
                    }
                    else return BadRequest();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post CreateToken/Authenticate: {ex}");
                return BadRequest("Failed to authenticate with CreateToken api");
            }
        }
    }
}
