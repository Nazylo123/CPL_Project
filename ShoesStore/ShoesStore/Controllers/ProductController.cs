using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;

using ShoesStore.ViewModel.ResponseModel;

using WebApi.Data;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProdudctRepository _productRepository;
        private AppDbContext _context;

		public ProductController(IProdudctRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
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

		


        // POST: api/Product
        [HttpPost("{categoryID}")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductRequestModel productRequest, int categoryID)
        {
            if (productRequest == null)
            {
                return BadRequest("ProductRequestModel is null.");
            }

            try
            {

                await _productRepository.AddProductAsync(productRequest, categoryID);
                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> Deleteproduct(int productId)
        {
            try
            {
                bool result = await _productRepository.DeleteproductAsync(productId);
                if (result)
                {
                    return Ok("Deleted successfully.");
                }
                else return NotFound($"Product with ID {productId} not found.");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductReponseViewModel>> GetProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(productId);

                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found.");
                }

                return Ok(product);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(int productId, [FromBody] ProductRequestModel productRequest)
        {
            if (productRequest == null)
            {
                return BadRequest("ProductRequestModel is null.");
            }

            try
            {
                await _productRepository.UpdateProdudctAsync(productRequest, productId);
                return Ok("Product updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
