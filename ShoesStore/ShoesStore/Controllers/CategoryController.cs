using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Data;
using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Get all categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            var categories = await _context.Categories
                                          .Select(c => new CategoryViewModel
                                          {
                                              Name = c.Name,
                                              Description = c.Description,
                                              ImageUrl = c.ImageUrl
                                          })
                                          .ToListAsync();

            return Ok(categories);
        }

        // 2. Get category by id
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategory(int id)
        {
            var category = await _context.Categories
                                        .Where(c => c.Id == id)
                                        .Select(c => new CategoryViewModel
                                        {
                                            Name = c.Name,
                                            Description = c.Description,
                                            ImageUrl = c.ImageUrl
                                        })
                                        .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // 3. Add a new category (Only Admin)
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryViewModel categoryViewModel)
        {
            var category = new Category
            {
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
                ImageUrl = categoryViewModel.ImageUrl,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // 4. Update category (Admin, Staff)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryViewModel categoryViewModel)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryViewModel.Name;
            category.Description = categoryViewModel.Description;
            category.ImageUrl = categoryViewModel.ImageUrl;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Delete category (Only Admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
