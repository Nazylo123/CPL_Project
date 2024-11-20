using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var product = _context.Products.ToList();
            return product;
        }
		[HttpGet("get-by-dat")]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductsByDatTest()
		{
			var products = await _context.Products
	   .Include(p => p.Category) // Liên kết với bảng Category
	   .Include(p => p.ProductSizeStocks) // Liên kết đến ProductSizeStock
            .ThenInclude(ps => ps.Size)
	   .Include(p => p.ProductImages) // Liên kết với bảng ProductImages
	   .Select(p => new ProductViewModel
	   {
		   Id = p.Id,
		   Name = p.Name,
		   Description = p.Description,
		   Price = p.Price,
		   CategoryId = p.CategoryId,
		   CategoryName = p.Category.Name,
		   CreatedAt = p.CreatedAt,
		   UpdatedAt = p.UpdatedAt,

		   // Lấy danh sách SizeId, SizeName, và Quantity từ ProductSizeStocks
		   SizeId = p.ProductSizeStocks.Select(pss => pss.SizeId).ToList(),
		   SizeName = p.ProductSizeStocks.Select(pss => pss.Size.SizeName).ToList(),
		   Quantity = p.ProductSizeStocks.Select(pss => pss.Quantity).ToList(),

		   // Dữ liệu hình ảnh
		   
		   Url = p.ProductImages.FirstOrDefault().ImageUrl
	   })
	   .ToListAsync();

			return Ok(products);
		}

		// GET: api/Product/{id}
		[HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return await product;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
