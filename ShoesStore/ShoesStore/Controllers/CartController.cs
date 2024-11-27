using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.Model;
using ShoesStore.ViewModel;
using ShoesStore.ViewModel.RequestModel;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Models;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly IProductSizeStockRepository _productSizeStockRepository;

		private readonly AppDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public CartController(IOrdersRepository ordersRepository, IProductSizeStockRepository productSizeStockRepository, AppDbContext context, UserManager<AppUser> userManager)
		{
			_ordersRepository = ordersRepository;
			_productSizeStockRepository = productSizeStockRepository;
		    _userManager = userManager;
			_context = context;
		}
		[HttpPost("checkout")]
		public async Task<IActionResult> Checkout([FromBody] CheckoutRequest checkoutRequest)
		{
			// Kiểm tra giỏ hàng
			var cart = checkoutRequest.Cart;
			if (cart == null || cart.Count == 0)
			{
				return BadRequest("Giỏ hàng trống");
			}

			// Kiểm tra email
			if (string.IsNullOrWhiteSpace(checkoutRequest.Email))
			{
				return BadRequest("Email không hợp lệ");
			}

			// Tìm thông tin người dùng
			var user = await _userManager.FindByEmailAsync(checkoutRequest.Email);
			if (user == null)
			{
				return BadRequest("Người dùng không tồn tại");
			}

			string userId = user.Id.ToString();

			// Tạo đơn hàng
			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.Now,
				Status = "Pending", // Trạng thái mặc định
				TotalAmount = cart.Sum(item => item.Price * item.Quantity),
				OrderItems = new List<OrderItem>()
			};

			// Xử lý từng mục trong giỏ hàng
			foreach (var item in cart)
			{
				// Thêm OrderItem
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
					return BadRequest($"Sản phẩm {item.ProductId} kích thước {item.SizeId} không đủ tồn kho.");
				}
			}

			// Lưu đơn hàng
			try
			{
				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi khi lưu đơn hàng: {ex.Message}");
			}

			// Chuẩn bị dữ liệu phản hồi
			var response = new
			{
				orderId = order.Id,
				userId = order.UserId,
				orderDate = order.OrderDate,
				status = order.Status,
				totalAmount = order.TotalAmount,
				items = order.OrderItems.Select(item => new
				{
					productId = item.ProductId,
					sizeId = item.SizeId,
					quantity = item.Quantity,
					price = item.Price
				})
			};

			return Ok(response);
		}


	}
}
