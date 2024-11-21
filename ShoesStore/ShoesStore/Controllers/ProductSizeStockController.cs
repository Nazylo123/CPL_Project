using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductSizeStockController : ControllerBase
	{
		private readonly IProductSizeStockRepository _productSizeStockRepository;

		public ProductSizeStockController(IProductSizeStockRepository productSizeStockRepository)
		{
			_productSizeStockRepository = productSizeStockRepository;
		}

		// Lấy thông tin tồn kho theo ProductId và SizeId
		[HttpGet("get-stock")]
		public async Task<IActionResult> GetStock([FromQuery] int productId, [FromQuery] int sizeId)
		{
			try
			{
				var stock = await _productSizeStockRepository.GetByProductAndSizeAsync(productId, sizeId);

				if (stock == null)
				{
					return NotFound(new
					{
						Message = "Sản phẩm hoặc kích thước không tồn tại trong kho.",
						ProductId = productId,
						SizeId = sizeId
					});
				}

				return Ok(stock);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					Message = "Có lỗi xảy ra khi lấy thông tin kho hàng.",
					Details = ex.Message
				});
			}
		}

		// Cập nhật tồn kho
		[HttpPut("update-stock")]
		public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequest request)
		{
			try
			{
				if (request == null || request.Quantity <= 0)
				{
					return BadRequest(new
					{
						Message = "Dữ liệu đầu vào không hợp lệ.",
						ErrorCode = 400
					});
				}

				var result = await _productSizeStockRepository.UpdateStockAsync(request.ProductId, request.SizeId, request.Quantity);

				if (!result)
				{
					return BadRequest(new
					{
						Message = "Không đủ số lượng tồn kho để thực hiện giao dịch.",
						ProductId = request.ProductId,
						SizeId = request.SizeId,
						QuantityRequested = request.Quantity
					});
				}

				return Ok(new
				{
					Message = "Cập nhật số lượng tồn kho thành công.",
					ProductId = request.ProductId,
					SizeId = request.SizeId,
					QuantityUpdated = request.Quantity
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					Message = "Có lỗi xảy ra khi cập nhật tồn kho.",
					Details = ex.Message
				});
			}
		}
	}
}
