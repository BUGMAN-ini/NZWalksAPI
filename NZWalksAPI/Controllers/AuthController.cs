using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO_s;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenrepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenrepository = tokenRepository;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var identityuser = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.UserName
            };

            var identityresult = await _userManager.CreateAsync(identityuser, registerDTO.Password);

            if (identityresult.Succeeded)
            {
                if (registerDTO.Roles != null && registerDTO.Roles.Any())
                {
                    identityresult = await _userManager.AddToRolesAsync(identityuser, registerDTO.Roles);

                    if (identityresult.Succeeded)
                    {
                        return Ok("User REgistered, Please Login");
                    }
                }
            }

            return BadRequest("Something Wrong");
        }


        //Login Method

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.UserName);
            if (user != null)
            {
                var checkpassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if(checkpassword)
                {
                    //Get Roles For This User
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        //Create Token

                        var jwttoken = _tokenrepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtResponse = jwttoken,
                        };
                        return Ok(response);
                    }
                }
            }

            return BadRequest("Incorrect User or Password");
        }


    }
}
