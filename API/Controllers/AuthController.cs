using API.Attributes;
using API.Errors;
using API.Extensions;
using Core.Dtos.Auth;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;

        public AuthController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await this.userManager
                .FindByEmailFromClaimsPrincipalAsync(this.User);

            return this.Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = this.tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(
            [FromQuery] string email)
        {
            return this.Ok(await this.userManager
                .FindByEmailAsync(email) != null);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.userManager
                .FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return this.Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
            }

            var result = await this.signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return this.Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
            }

            return this.Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = this.tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await this.userManager
                .FindByEmailAsync(registerDto.Email) != null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email address is already in use" }
                });
            }

            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };

            var result = await this.userManager
                .CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return this.BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            return this.Ok(new UserDto
            {
                Id = user.Id,
                Token = this.tokenService.CreateToken(user),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }
    }
}
