using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductSizeStockController : ControllerBase
	{
		private readonly IProductSizeStockRepository _productSizeStockRepository;
		private readonly AppDbContext _context;
		public ProductSizeStockController(IProductSizeStockRepository productSizeStockRepository,AppDbContext context)
		{
			_productSizeStockRepository = productSizeStockRepository;
			_context = context;
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
		[HttpGet("get-all-product-sizes")]
		public async Task<IActionResult> GetAllProductSizes()
		{
			try
			{
				// Lấy tất cả Size và các thông tin liên quan từ ProductSizeStocks
				var productSizes = await _context.Sizes
					.Include(p => p.ProductSizeStocks)  // Bao gồm ProductSizeStocks
					.Select(p => new SizeViewModel
					{
						Id = p.Id,  // Mã size
						SizeName = p.SizeName,  // Tên kích cỡ
						ProductId = p.ProductSizeStocks.Select(pss => pss.ProductId).ToList(),  // Lấy danh sách ProductId từ ProductSizeStocks
						Quantity = p.ProductSizeStocks.Select(pss => pss.Quantity).ToList()  // Lấy danh sách Quantity từ ProductSizeStocks
					})
					.ToListAsync();

				if (productSizes == null || productSizes.Count == 0)
				{
					return NotFound("Không có dữ liệu.");
				}

				// Trả về danh sách ProductSize với các thông tin liên quan
				return Ok(productSizes);
			}
			catch (Exception ex)
			{
				// Log lỗi và trả về lỗi
				return StatusCode(500, "Lỗi khi lấy dữ liệu: " + ex.Message);
			}
		}











	}
}
