using ShoesStore.Model;

namespace ShoesStore.IRepository
{
	public interface IProductSizeStockRepository
	{
		Task<ProductSizeStock> GetByProductAndSizeAsync(int productId, int sizeId);
		Task<bool> UpdateStockAsync(int productId, int sizeId, int quantity);

	}
}
