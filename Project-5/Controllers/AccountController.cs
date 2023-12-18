using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project_5.Models;
using Project_5.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Project_5.DAL;
using System.Data;
using Project_5.Extension;
using Microsoft.EntityFrameworkCore;

namespace Project_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,

            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;

            _db = db;
        }

        [HttpPost("LoginForUsers")]
        
       
        public async Task<ActionResult<LoginResultVM>> LoginForUsers(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            AppUser appUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.Username);
            if (appUser == null)
            {
                    return NotFound(new
                    {
                        message = "Email or password are incorrect!"
                    });
            }
            if (appUser.IsDeactive)
            {
                return NotFound(new
                {
                    message = "Your account has blocekd"
                });
            }
            
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                return NotFound(new
                {
                    message = "Your account has locked for 1 min!"
                }); ;
            }
            if (!signInResult.Succeeded)
            {
                return NotFound(new
                {
                    message = "Email or password are incorrect!"
                });
            }
            if (!User.IsInRole("Member"))
            {
                return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Error", Message = "Your role is not user" });
               
            }

            var token = GenerateJwtToken(appUser);
            return Ok(new LoginResultVM
            {
                UserId = appUser.Id,
                AuthToken = token
            });
        }

        [HttpPost("LoginForAdmins")]


        public async Task<ActionResult<LoginResultVM>> LoginForAdmins(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            AppUser appUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.Username);
            if (appUser == null)
            {
                return NotFound(new
                {
                    message = "Email or password are incorrect!"
                });
            }
            if (appUser.IsDeactive)
            {
                return NotFound(new
                {
                    message = "Your account has blocekd"
                });
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                return NotFound(new
                {
                    message = "Your account has locked for 1 min!"
                }); ;
            }
            if (!signInResult.Succeeded)
            {
                return NotFound(new
                {
                    message = "Email or password are incorrect!"
                });
            }
            if (!User.IsInRole("Admin"))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Your role is not admin" });

            }

            var token = GenerateJwtToken(appUser);
            return Ok(new LoginResultVM
            {
                UserId = appUser.Id,
                AuthToken = token
            });
        }

        [HttpPost("LoginForCompanies")]
        public async Task<ActionResult<LoginResultVM>> LoginForCompanies(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            AppUser appUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.Username);
            if (appUser == null)
            {
                return NotFound(new
                {
                    message = "Email or password are incorrect!"
                });
            }
            if (appUser.IsDeactive)
            {
                return NotFound(new
                {
                    message = "Your account has blocekd"
                });
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                return NotFound(new
                {
                    message = "Your account has locked for 1 min!"
                }); ;
            }
            if (!signInResult.Succeeded)
            {
                return NotFound(new
                {
                    message = "Email or password are incorrect!"
                });
            }
            if (!User.IsInRole("Company"))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Your role is not company admin" });

            }

            var token = GenerateJwtToken(appUser);
            return Ok(new LoginResultVM
            {
                UserId = appUser.Id,
                AuthToken = token
            });
        }

        [HttpPost("Register")]
     
        public async Task<IActionResult> Register([FromBody]RegisterVM registerVM)
        {
            AppUser newUser = new AppUser
            {
                
                Firstname = registerVM.Firstname,
                Lastname = registerVM.Lastname,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                Adress = registerVM.Adress,
                Phone = registerVM.Phone,


            };
            var identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
              
                await _db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Account created successfully" });
            }
            else
            {
            
                return BadRequest(new Response { Status = "Error", Message = "Failed to create user", Errors = identityResult.Errors.Select(e => e.Description).ToList() });
            }

          
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            }
            else if (!(await _roleManager.RoleExistsAsync(Roles.Member.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Company.ToString() });

            }
            return Content("Done");

        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Logout", Message = "Process done succesfully" });
        }


        private string GenerateJwtToken(AppUser user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("My security key with more bits12");


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name , user.Firstname)
                }),


                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

