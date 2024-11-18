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

        public ProductController(IProdudctRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReponseViewModel>>> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("No products found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
