using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Models.Authentication.Login;
using ObjectDetectionAPI.Models.Authentication.SignUp;
using ObjectDetectionAPI.Services;
using System.Security.Claims;

namespace ObjectDetectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly FileStoreService _fileStoreService;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,TokenService tokenService, FileStoreService fileStoreService)
        {
            _userManager=userManager;
            _roleManager = roleManager;
            _configuration=configuration;
            _tokenService = tokenService;
            _fileStoreService = fileStoreService;
        }
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            //Check User Exist
            var userExist = await _userManager.FindByNameAsync(registerUser.Username);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "User already exists!" });
            }
            //Add user in db
            IdentityUser user = new IdentityUser()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username
            };

                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                     return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User failed to create!" });
                }
                //assign a role
                await _userManager.AddToRoleAsync(user, "User");

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User Created Successfully!" });


        }


        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            var userExist = await _userManager.FindByNameAsync(loginUser.Username);
            if (userExist == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
            }
            var result = await _userManager.CheckPasswordAsync(userExist,loginUser.Password);//PasswordSignInAsync(Input.Email,
            if (!result)
            {
                StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password Incorrect" });
            }
            string accessToken = _tokenService.CreateToken(userExist);
            return StatusCode(StatusCodes.Status200OK,
                accessToken);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("get-my-images")]
        public async Task<IActionResult> GetUserImages()
        {
            var images = await _fileStoreService.GetImagesByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return
                StatusCode(StatusCodes.Status200OK, images);
            
        }
    }
}
