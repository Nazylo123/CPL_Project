using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProdudctImageRepository _productImageRepository;

        public ProductImageController(IProdudctImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        // POST: api/ProductImage/{productID}
        [HttpPost("{productID}")]
        public async Task<ActionResult<ProductImageRequestModel>> AddImage(int productID, [FromBody] ProductImageRequestModel productImageRequestModel)
        {
            if (productImageRequestModel == null)
            {
                return BadRequest("ProductImageRequestModel is null.");
            }

            try
            {
                var newImage = await _productImageRepository.AddImageAsync(productID, productImageRequestModel);

                if (newImage == null)
                {
                    return NotFound($"Product with ID {productID} not found.");
                }

                return CreatedAtAction(nameof(GetImageById), new { imageID = newImage.ProductId }, newImage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/ProductImage/{imageID}
        [HttpDelete("{imageID}")]
        public async Task<ActionResult> DeleteImage(int imageID)
        {
            try
            {
                var result = await _productImageRepository.DeleteImageAsync(imageID);

                if (!result)
                {
                    return NotFound($"Image with ID {imageID} not found.");
                }

                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/ProductImage/product/{productID}
        [HttpGet("product/{productID}")]
        public async Task<ActionResult<IEnumerable<ProductImageRequestModel>>> GetAllImagesByProductId(int productID)
        {
            try
            {
                var images = await _productImageRepository.GetAllProductImageAsync(productID);

                if (!images.Any())
                {
                    return NotFound($"No images found for product with ID {productID}.");
                }

                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/ProductImage/{imageID}
        [HttpGet("{imageID}")]
        public async Task<ActionResult<ProductImageRequestModel>> GetImageById(int imageID)
        {
            try
            {
                var image = await _productImageRepository.GetProductImageByIdAsync(imageID);

                if (image == null)
                {
                    return NotFound($"Image with ID {imageID} not found.");
                }

                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/ProductImage/{imageID}
        [HttpPut("{imageID}")]
        public async Task<ActionResult<ProductImageRequestModel>> UpdateImage(int imageID, [FromBody] ProductImageRequestModel productImageRequestModel)
        {
            if (productImageRequestModel == null)
            {
                return BadRequest("ProductImageRequestModel is null.");
            }

            try
            {
                var updatedImage = await _productImageRepository.UpdateImageAsync(imageID, productImageRequestModel);

                if (updatedImage == null)
                {
                    return NotFound($"Image with ID {imageID} not found.");
                }

                return Ok(updatedImage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
