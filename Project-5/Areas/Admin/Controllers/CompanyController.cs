using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Models;
using Project_5.ViewModel;

namespace Project_5.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CompanyController : ControllerBase
    {

        private readonly AppDbContext _db;


        public CompanyController(AppDbContext db)
        {
            _db = db;

        }
        [HttpGet("GetAllCompanies")]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _db.Cateogries.Where(c=>c.IsDeactive==false).ToListAsync();
            return Ok(companies);
        }
        [HttpGet("GetCmopanyById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            var company = await _db.Cateogries.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find company with this id" });
            }
            return Ok(company);
        }
        [HttpPut("UpdateCompany/{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyVM company)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            var dbcompany = await _db.Companies.FirstOrDefaultAsync(m => m.Id == id);
            if (dbcompany == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find company with this id" });
            }
            var existcompany = await _db.Companies.FirstOrDefaultAsync(c => c.Name == company.Name);
            if (existcompany != null) { return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Company with this name is already exist" }); }
            dbcompany.Name = company.Name;
            dbcompany.Description = company.Description;
            await _db.SaveChangesAsync();
            return Ok(company);
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyVM company)
        {
            if (company == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Company cant be null" });
            }
            var existcompany = await _db.Companies.FirstOrDefaultAsync(c => c.Name == company.Name);
            if (existcompany != null) { return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Company with this name is already exist" }); }
            Project_5.Models.Company newcompany = new Project_5.Models.Company()
            {
                Name = company.Name,
                Description = company.Description,
            };

            await _db.Companies.AddAsync(newcompany);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Careated", Message = "Company has created!" });
        }
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id cant be null" });
            }
            var deletedcompany = await _db.Companies.FirstOrDefaultAsync(m => m.Id == id);
            if (deletedcompany == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find category with this id" });
            }
            var measure = await _db.Measures.FirstOrDefaultAsync(m => m.CompanyId == id);
            if(measure != null) { measure.IsDeactive = true; }
           
            deletedcompany.IsDeactive = true;
            
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Deleted", Message = "Company has deleted!" });
        }
    }
}
