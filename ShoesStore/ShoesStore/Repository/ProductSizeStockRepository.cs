using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.Model;
using WebApi.Data;

namespace ShoesStore.Repository
{
	public class ProductSizeStockRepository : IProductSizeStockRepository
	{
		private readonly AppDbContext _context;

		public ProductSizeStockRepository(AppDbContext context)
		{
			_context = context;
		}

		// Lấy thông tin tồn kho của sản phẩm theo kích thước
		public async Task<ProductSizeStock> GetByProductAndSizeAsync(int productId, int sizeId)
		{
			return await _context.ProductSizeStocks
					.FirstOrDefaultAsync(pss => pss.ProductId == productId && pss.SizeId == sizeId);
		}

		// Cập nhật số lượng tồn kho
		public async Task<bool> UpdateStockAsync(int productId, int sizeId, int quantity)
		{
			var stock = await GetByProductAndSizeAsync(productId, sizeId);

			if (stock == null || stock.Quantity < quantity)
			{
				// Trả về false nếu số lượng trong kho không đủ
				return false;
			}

			// Nếu đủ số lượng, thực hiện các thao tác khác như thêm vào đơn hàng, giảm số lượng tồn kho, v.v.
			stock.Quantity -= quantity; // Giảm số lượng tồn kho theo số lượng đã mua
			_context.ProductSizeStocks.Update(stock);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
