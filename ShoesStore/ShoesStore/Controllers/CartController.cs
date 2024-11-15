using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.Model;
using ShoesStore.ViewModel;
using ShoesStore.ViewModel.RequestModel;
using System.Security.Claims;
using WebApi.Data;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly IProductSizeStockRepository _productSizeStockRepository;

		private readonly AppDbContext _context;

		public CartController(IOrdersRepository ordersRepository, IProductSizeStockRepository productSizeStockRepository, AppDbContext context)
		{
			_ordersRepository = ordersRepository;
			_productSizeStockRepository = productSizeStockRepository;
		
			_context = context;
		}

		[HttpPost("checkout")]
		public async Task<IActionResult> Checkout([FromBody] CheckoutRequest checkoutRequest)
		{
			var cart = checkoutRequest.Cart; // Cart từ frontend gửi lên

			if (cart == null || cart.Count == 0)
			{
				return BadRequest("Giỏ hàng trống");
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy UserId từ JWT token

			// Tạo đơn hàng
			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.UtcNow,
				Status = "Pending", // Hoặc trạng thái khác như "Completed"
				TotalAmount = cart.Sum(item => item.Price * item.Quantity),
				OrderItems = new List<OrderItem>()
			};

			// Tạo các mục trong đơn hàng và cập nhật tồn kho
			foreach (var item in cart)
			{
				// Thêm OrderItem vào đơn hàng
				var orderItem = new OrderItem
				{
					ProductId = item.ProductId,
					SizeId = item.SizeId,
					Quantity = item.Quantity,
					Price = item.Price
				};
				order.OrderItems.Add(orderItem);

				// Cập nhật tồn kho
				bool isStockUpdated = await _productSizeStockRepository.UpdateStockAsync(item.ProductId, item.SizeId, item.Quantity);
				if (!isStockUpdated)
				{
					// Nếu không đủ kho, trả về lỗi và không tiếp tục xử lý đơn hàng
					return BadRequest($"Số lượng sản phẩm {item.ProductId} kích thước {item.SizeId} không đủ trong kho.");
				}
			}

			// Lưu đơn hàng và các mục của nó
			try
			{
				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Nếu có lỗi khi lưu đơn hàng, trả về lỗi
				return StatusCode(500, $"Lỗi khi lưu đơn hàng: {ex.Message}");
			}

			// Xóa giỏ hàng trong cookie sau khi thanh toán
			Response.Cookies.Delete("cart");

			return Ok(order);
		}

	}
}
