using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System;
using Project_5.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Project_5.Helpers;
using Project_5.Models;
using Microsoft.AspNetCore.Authorization;
using Project_5.ViewModel;

namespace Project_5.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class MeasureController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public MeasureController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        [HttpGet("GetAll")]

        //[MyAuthorize]
        public async Task<ActionResult> GetAll()
        {
            var measures = await _db.Measures.Where(m=>m.IsDeactive==false).ToListAsync();
           
            return Ok(measures);


        }
        [HttpGet("GetById/{id}")]

        public async Task<ActionResult> GetById(int? id)

        {
            if (id == null)
            {
                return BadRequest();
            }
            var measure = await _db.Measures.FirstOrDefaultAsync(x => x.Id == id);
            if (measure == null)
            {

                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            return Ok(measure);


        }
        [HttpPut("Update/{id}")]

        public async Task<ActionResult> Update(int id, MeasureVM measure)
        {

            var dbmeasure = await _db.Measures.FirstOrDefaultAsync(m => m.Id == id);
            if (dbmeasure == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Can;t find measure" });
            }
            var existmeasure = _db.Measures.FirstOrDefault(m => m.Title == measure.Title);
            if (existmeasure != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Measure with this name is already exist" });
            }
            dbmeasure.Title = measure.Title;
            dbmeasure.Description = measure.Description;
            dbmeasure.Adress = measure.Adress;
            dbmeasure.DateTime = measure.DateTime;
            dbmeasure.Status = measure.Status;
            dbmeasure.Accessibility = measure.Accessibility;
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Updated", Message = "Measure is updated" });
        }
        [HttpDelete("DeleteMeasure/{id}")]

        public async Task<IActionResult> Delete(int? id)
        {

            var measure = await _db.Measures.FirstOrDefaultAsync(x => x.Id == id);
          
            if (measure == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            measure.IsDeactive = true;
           
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Ok", Message = "Measure has been deleted" });
        }
        [HttpPost("CreateMeasure")]
        public async Task<IActionResult> Create(CreateMeasureVM measure)
        {
            if (measure == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Measure cant be null" });
            }
            var existmeasure = _db.Measures.FirstOrDefault(m => m.Title == measure.Title);
            if (existmeasure != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Measure with this name is already exist" });
            }
            var url = Measure.GenerateUniqueUrl();
            Measure addmeasure = new Measure()
            {
                Title = measure.Title,
                Adress = measure.Adress,
                Description = measure.Description,
                DateTime = measure.DateTime,
                CategoryId = measure.CategoryId,
                CompanyId = measure.CompanyId,
                TicketCount = measure.TicketCount,
                Accessibility = measure.Accessibility,
                Status = "Upcoming"
                
            };
            addmeasure.URL = url;
            

            await _db.Measures.AddAsync(addmeasure);
            await _db.SaveChangesAsync();
            if(addmeasure.Accessibility == "Private")
            {
                return Ok(new
                {
                    Status = "Created",
                    Message = "Measure has created . Only those with whom you share the link can buy tickets.",
                    URL = $"{addmeasure.URL}"
                }) ;
            }
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Ok", Message = "Measure has been created" });
        }

    }
}
