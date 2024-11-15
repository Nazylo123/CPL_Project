using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;

namespace ShoesStore.Repository
{
	public class OrderItemsRepository : IOrderItemsRepository
	{
		private readonly AppDbContext _context;

		public OrderItemsRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<OrderItemViewModel>> GetAllOrderItemsAsync()
		{
			return await _context.OrderItems
				.Include(oi => oi.Product)
				.Include(oi => oi.Size)
				.Select(oi => new OrderItemViewModel
				{
					Id = oi.Id,
					OrderId = oi.OrderId,
					ProductId = oi.ProductId,
					SizeId = oi.SizeId,
					Quantity = oi.Quantity,
					Price = oi.Price,
					ProductName = oi.Product.Name, // Tên sản phẩm từ bảng Product
					SizeName = oi.Size.SizeName // Tên kích thước từ bảng Size
				})
				.ToListAsync();
		}

		public async Task<OrderItemViewModel> GetOrderItemByIdAsync(int orderItemId)
		{
			var orderItem = await _context.OrderItems
				.Include(oi => oi.Product)
				.Include(oi => oi.Size)
				.FirstOrDefaultAsync(oi => oi.Id == orderItemId);

			if (orderItem == null) return null;

			return new OrderItemViewModel
			{
				Id = orderItem.Id,
				OrderId = orderItem.OrderId,
				ProductId = orderItem.ProductId,
				SizeId = orderItem.SizeId,
				Quantity = orderItem.Quantity,
				Price = orderItem.Price,
				ProductName = orderItem.Product.Name,
				SizeName = orderItem.Size.SizeName
			};
		}

		public async Task AddOrderItemAsync(OrderItemViewModel orderItemViewModel)
		{
			var orderItem = new OrderItem
			{
				OrderId = orderItemViewModel.OrderId,
				ProductId = orderItemViewModel.ProductId,
				SizeId = orderItemViewModel.SizeId,
				Quantity = orderItemViewModel.Quantity,
				Price = orderItemViewModel.Price
			};

			_context.OrderItems.Add(orderItem);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateOrderItemAsync(OrderItemViewModel orderItemViewModel)
		{
			var orderItem = await _context.OrderItems.FindAsync(orderItemViewModel.Id);
			if (orderItem != null)
			{
				orderItem.Quantity = orderItemViewModel.Quantity;
				orderItem.Price = orderItemViewModel.Price;
				orderItem.SizeId = orderItemViewModel.SizeId;

				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteOrderItemAsync(int orderItemId)
		{
			var orderItem = await _context.OrderItems.FindAsync(orderItemId);
			if (orderItem != null)
			{
				_context.OrderItems.Remove(orderItem);
				await _context.SaveChangesAsync();
			}
		}
	}
}
