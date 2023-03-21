using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ObjectDetectionAPI.Models;
using ObjectDetectionAPI.Models.Authentication.SignUp;

namespace ObjectDetectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager=userManager;
            _roleManager = roleManager;
            _configuration=configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser,string role)
        {
            //Check User Exist
            var userExist = await _userManager.FindByNameAsync(registerUser.Email);
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
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                     return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User failed to create!" });
                }
                //assign a role
                await _userManager.AddToRoleAsync(user, role);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User Created Successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Role does not exist!" });

            }


        }
    }
}
