using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Models;

namespace Project_5.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public StatisticsController(AppDbContext db , UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet("GetStatistics/{id}")]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var user =await  _userManager.GetUserAsync(User);
            var measure =await  _db.Measures.Include(m=>m.Users).FirstOrDefaultAsync(m=>m.Id == id );
            if (measure != null)
            {
                NotFound();
            }
            if(user.CompanyId != measure.CompanyId)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "You can look at statistics of your company!" });
            }
            return Ok(new
            {
                ParticipantsCount = measure.Users.Count,
                DateTime = measure.DateTime,
                Status = measure.Status,
                Accessibility = measure.Accessibility
            });
        }
    }
}
