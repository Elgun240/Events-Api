using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Project_5.Models;
using Project_5.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Project_5.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public CategoryController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _db.Cateogries.Where(c=>c.IsDeactive==false).ToListAsync();
            return Ok(categories);
        }
        [HttpGet("GetCatById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            var category = await _db.Cateogries.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find category with this id" });
            }
            return Ok(category);
        }
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryVM category)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id can not be null" });
            }
            var existcategory = await _db.Cateogries.FirstOrDefaultAsync(c => c.Name == category.Name);
            if (existcategory != null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Category with this name is already exist" });
            }
            var dbcateogry = await _db.Cateogries.FirstOrDefaultAsync(m => m.Id == id);
            if (dbcateogry == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find categroy with this id" });
            }
            dbcateogry.Name = category.Name;
            await _db.SaveChangesAsync();
            return Ok(category);
        }
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryVM category)
        {
            if (category == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Category cant be null" });
            }
            var existcategory = await _db.Cateogries.FirstOrDefaultAsync(c => c.Name == category.Name);
            if (existcategory != null) { return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Category with this name is already exist" }); }
            Category newcategory = new Category()
            {
                Name = category.Name,
            };
            await _db.Cateogries.AddAsync(newcategory);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Careated", Message = "Category has created!" });
        }
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Id cant be null" });
            }
            var deletedcateogry = await _db.Cateogries.FirstOrDefaultAsync(m => m.Id == id);
            if (deletedcateogry == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Cant find category with this id" });
            }
            var measures = await _db.Measures.Where(m => m.CategoryId == id).ToListAsync();
            foreach (var item in measures)
            {
                item.IsDeactive = true;
            }
           deletedcateogry.IsDeactive= true;
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Deleted", Message = "Category has deleted!" });
        }
    }
}
