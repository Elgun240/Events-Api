using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Helpers;
using Project_5.Models;
using MimeKit.IO;
using Project_5.ViewModel;

namespace Project_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Member")]
    public class ShopController : Controller
    {
        private readonly AppDbContext _db;
        
        private readonly UserManager<AppUser> _userManager;
       
        public ShopController(AppDbContext db, 
            IConfiguration configuration,
            UserManager<AppUser> usermanager)
        {
            _db = db;
            _userManager = usermanager;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var measures = await _db.Measures.Include(m => m.Company).Where(m=>m.IsDeactive==false).ToListAsync();
            List<MeasureVM> allmeasures = new List<MeasureVM>();
            foreach (var item in measures)
            {
                if(item.Accessibility != "Private")
                {
                    MeasureVM measureVM = new MeasureVM()
                    {
                        Title = item.Title,
                        Id = item.Id,
                        Description = item.Description,
                        DateTime = item.DateTime,
                        Status = item.Status,
                        CompanyName = item.Company.Name
                    };
                    allmeasures.Add(measureVM);
                }
            }
            return Ok(allmeasures);


        }
        [HttpGet("BuyTicket/{id?}")]
        public async Task<IActionResult> BuyTicket(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var ticket = await _db.Tickets.Include(t=>t.Measure).FirstOrDefaultAsync(x => x.Id == id && x.Measure.IsDeactive==false);
            if (ticket == null)
            {

                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            if (ticket.Measure.TicketCount < 1)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "All tickets sold" });
            }
           
            AppUser user = await _userManager.GetUserAsync(User);
            if (user.Balance < ticket.Price)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Insufficient funds .Please top up your balance" });
            }
            user.Balance -= ticket.Price;
            ticket.Measure.TicketCount -= 1;
            AppUserMeasure appUserMeasure = new AppUserMeasure()
            {
                MeasureId = ticket.MeasureId,
                AppUserId = user.Id,
            };
            await _db.AppUserMeasures.AddAsync(appUserMeasure);
            SendMail(user,ticket.Measure);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted, new Response { Status = "Done", Message = "Congratulations on your ticket purchase" });
        }
        [HttpGet("ByTicketbyLink/{link?}")]
        public async Task<IActionResult> ByWithLink(string link)
        {
            if (link == null)
            {
                return BadRequest();
            }
            var measure = await _db.Measures.FirstOrDefaultAsync(m=>m.URL == link);
            if (measure == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Incorrect link" });
            }
            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.MeasureId == measure.Id);
            if (ticket == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "No tickets for this measure" });
            }
            if (measure.TicketCount<1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "No tickets left" });
            }
            AppUser user = await _userManager.GetUserAsync(User);
         
            ticket.Measure.TicketCount -= 1;
            AppUserMeasure appUserMeasure = new AppUserMeasure()
            {
                MeasureId = ticket.MeasureId,
                AppUserId = user.Id,
            };
            await _db.AppUserMeasures.AddAsync(appUserMeasure);
            SendMail(user, ticket.Measure);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted, new Response { Status = "Done", Message = "Congratulations on your ticket purchase" });
        }
        //[HttpPost("ShowTicket")]
        //public async Task<IActionResult> ShowTicket(int ticketId)
        //{
        //    var ticket = await _db.Tickets.FirstOrDefaultAsync(t=>t.Id==ticketId);
        //    var meassure = await _db.Measures.FirstOrDefaultAsync(t=>t.Id==ticket.MeasureId);
        //    var ticketData = new { TicketId = ticketId, TicketPrice = ticket.Price , MeasureName = meassure.Title};

        //    return View(ticketData);
        //}
        private async Task SendMail(AppUser user , Measure measure)
        {
           
            string pdfFilePath = Path.GetTempFileName();

            try
            {
                
               
                    using (Document pdf = new Document())
                    {
                        PdfWriter.GetInstance(pdf, new FileStream(pdfFilePath,FileMode.Create));

                    pdf.Open();
                        pdf.Add(new Paragraph($"Company name: {measure.Title}"));
                        pdf.Add(new Paragraph($"Date: {measure.DateTime}"));
                        pdf.Add(new Paragraph($"User: {user.UserName}"));
                        pdf.Add(new Paragraph($"Email: {user.Email}"));
                    string qrCodeContent = $"Пользователь: {user.UserName}, Email: {user.Email}";
                    iTextSharp.text.Image qrCodeImage = GenerateQRCodeImage(qrCodeContent, 100); 
                    pdf.Add(qrCodeImage);
                    pdf.Close();
                    }
                

                
                string toEmail = user.Email;
                string subject = "Ticket";
                string message = "Your ticket details are attached.";

                IEmailService emailService = new EmailService();
                await emailService.SendEmailWithAttachmentAsync(toEmail, subject, message, pdfFilePath);
                
            }
            finally
            {
                
              System.IO.File.Delete(pdfFilePath);
                
            }
        }
        private iTextSharp.text.Image GenerateQRCodeImage(string content, int size)
        {
            BarcodeQRCode qrCode = new BarcodeQRCode(content, size, size, null);
            Image qrCodeImage = qrCode.GetImage();
            return qrCodeImage;
        }
      
    }
    
}
