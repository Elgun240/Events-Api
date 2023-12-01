using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Models;
using Project_5.ViewModel;

namespace Project_5.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Company")]
    public class TicketController : Controller
    {
        private readonly AppDbContext _db;
        public TicketController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("GetAllTicekts")]
        public async Task<IActionResult> GetAll()
        {
            var ticekts = _db.Tickets.Include(t => t.Measure).ToList();
            return Ok(ticekts);
        }
        [HttpPut("ChangePrice/{id}")]
        public async Task<IActionResult> ChangePrice(int id, double newPrice)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id cant be null" });

            }
            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Not Found", Message = "Cant found ticekt with this id" });
            }
            ticket.Price = newPrice;
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Cahnged", Message = "Price changed succesfully!" });
        }
        [HttpPost("CreateTicket")]
        public async Task<IActionResult> CreateTicket(CreateTicketVM newTicket)
        {
            if (newTicket == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Model can not be null" });
            }
            var existmeasure = await _db.Measures.FirstOrDefaultAsync(m => m.Id == newTicket.MeasureId);
            if (existmeasure == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Not found measure with this id" });
            }
            if (newTicket.Price < 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Price must be positive number" });
            }
            Ticket ticket = new Ticket()
            {
                MeasureId = newTicket.MeasureId,
                Price = newTicket.Price,
            };
            await _db.Tickets.AddAsync(ticket);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Ok", Message = "Ticket has been created" });
        }
        [HttpDelete("DeleteTicket/{id}")]
        public async Task<IActionResult> DeletTicket(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id cant be null" });
            }
            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Not found ticket with this id" });
            }
            _db.Tickets.Remove(ticket);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted, new Response { Status = "Deleted", Message = "Ticket was deleted successly" });
        }
    }
}
