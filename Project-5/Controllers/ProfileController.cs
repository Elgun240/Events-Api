using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Extension;
using Project_5.Models;
using Project_5.ViewModel;

namespace Project_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        public ProfileController(AppDbContext db,
            UserManager<AppUser> usermanager)
        {
            _userManager = usermanager;
            _db = db;
        }
        [HttpGet("CheckInfo")]
        public async Task<IActionResult> ChechInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            UserInfo userInfo = new UserInfo()
            {
                Firstname = user.Firstname,
                Lastename = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.UserName,

            };
            return Ok(userInfo);
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (changePasswordVM.CurrentPassword == null || changePasswordVM.NewPassword == null || changePasswordVM.CheckPassword == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Fill in the field!" });
            }
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
            if (result.Succeeded)
            {

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Changed", Message = "Password changed succesfully" });
            }
            else
            {

                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Incorrect format of password" });
            }

        }
        [HttpGet("LookAtTcikets")]
        public async Task<IActionResult> LookAtTickets()
        {
            var user = await _userManager.GetUserAsync(User);
            var tickets = await _db.Tickets.Where(t => t.AppUserId == user.Id).ToListAsync();
            List<UserTicketsVM> alltickets = new List<UserTicketsVM>();
            foreach (var ticketsVM in tickets)
            {
                UserTicketsVM ticketVM = new UserTicketsVM()
                {
                    TicketId = ticketsVM.Id,
                    Price = ticketsVM.Price,
                };
                alltickets.Add(ticketVM);
            }
            return Ok(alltickets);
        }
       
    }
}
