using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Models;
using Project_5.ViewModel;
using System.Reflection.Metadata.Ecma335;

namespace Project_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Member")]
    public class CommentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(AppDbContext db,UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        [HttpPost("SendComment")]
       
        public async Task<IActionResult> Send( SendCommentVM comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            AppUser appUser = await _userManager.GetUserAsync(User);

            var isParticipant = _db.AppUserMeasures.Any(a=>a.MeasureId==comment.MeasureId && a.AppUserId==appUser.Id);
            if (!isParticipant)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Only participants can write comment" });
            }
            var measure = await _db.Measures.FirstOrDefaultAsync(m=>m.Id ==comment.MeasureId);
            if(measure.Status == "Done")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "YOu can send message only after ending measure" });
            }
            Comment ncomment = new Comment()
            {
                MeasureId = comment.MeasureId,
                Title = comment.Title,
                Description = comment.Description,
                AppUserId = appUser.Id,
                CreateTime = DateTime.Now,

            };
            await _db.Comments.AddAsync(ncomment);
            await _db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted, new Response { Status = "Sended", Message = "Comment was sent" });   
        }
    }
}
