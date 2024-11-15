using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Model;
using WebApi.Data;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductImageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductImage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductImage>>> GetProductImages()
        {
            var productImages = await _context.ProductImages.ToListAsync();
            return Ok(productImages);
        }

        // GET: api/ProductImage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductImage>> GetProductImage(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);

            if (productImage == null)
            {
                return NotFound();
            }

            return Ok(productImage);
        }

        // POST: api/ProductImage
        [HttpPost]
        public async Task<ActionResult<ProductImage>> PostProductImage(ProductImage productImage)
        {
            // Kiểm tra xem sản phẩm đã tồn tại hay chưa (nếu cần thiết)
            var product = await _context.Products.FindAsync(productImage.ProductId);
            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            // Thêm mới productImage vào cơ sở dữ liệu
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductImage", new { id = productImage.Id }, productImage);
        }

        // PUT: api/ProductImage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductImage(int id, ProductImage productImage)
        {
            if (id != productImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(productImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductImageExists(id))
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

        // DELETE: api/ProductImage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return NotFound();
            }

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.Id == id);
        }
    }
}
