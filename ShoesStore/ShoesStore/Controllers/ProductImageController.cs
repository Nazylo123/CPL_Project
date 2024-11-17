using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Model;
using WebApi.Data;
using ShoesStore.DTO;

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
        public async Task<ActionResult<IEnumerable<ProductImageDTO>>> GetProductImages()
        {
            var productImages = await _context.ProductImages
                .Select(pi => new ProductImageDTO
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl,
                    ProductId = pi.ProductId
                })
                .ToListAsync();

            return Ok(productImages);
        }

        // GET: api/ProductImage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductImageDTO>> GetProductImage(int id)
        {
            var productImage = await _context.ProductImages
                .Where(pi => pi.Id == id)
                .Select(pi => new ProductImageDTO
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl,
                    ProductId = pi.ProductId
                })
                .FirstOrDefaultAsync();

            if (productImage == null)
            {
                return NotFound(new { Message = "Product image not found." });
            }

            return Ok(productImage);
        }

        // POST: api/ProductImage
        [HttpPost]
        public async Task<ActionResult<ProductImageDTO>> PostProductImage(ProductImageDTO productImageDto)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == productImageDto.ProductId);
            if (!productExists)
            {
                return BadRequest(new { Message = "Product not found." });
            }

            var productImage = new ProductImage
            {
                ImageUrl = productImageDto.ImageUrl,
                ProductId = productImageDto.ProductId
            };

            try
            {
                _context.ProductImages.Add(productImage);
                await _context.SaveChangesAsync();

                productImageDto.Id = productImage.Id; // Gán ID sau khi thêm
                return CreatedAtAction(nameof(GetProductImage), new { id = productImage.Id }, productImageDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Error adding product image.", Details = ex.Message });
            }
        }

        // PUT: api/ProductImage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductImage(int id, ProductImageDTO productImageDto)
        {
            if (id != productImageDto.Id)
            {
                return BadRequest(new { Message = "ID mismatch." });
            }

            var existingProductImage = await _context.ProductImages.FindAsync(id);
            if (existingProductImage == null)
            {
                return NotFound(new { Message = "Product image not found." });
            }

            try
            {
                existingProductImage.ImageUrl = productImageDto.ImageUrl;
                existingProductImage.ProductId = productImageDto.ProductId;

                _context.Entry(existingProductImage).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Product image updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Error updating product image.", Details = ex.Message });
            }
        }

        // DELETE: api/ProductImage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return NotFound(new { Message = "Product image not found." });
            }

            try
            {
                _context.ProductImages.Remove(productImage);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Product image deleted successfully.", DeletedProductImageId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Error deleting product image.", Details = ex.Message });
            }
        }

        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.Id == id);
        }
    }
}
