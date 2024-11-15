using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.IRepository
{
	public interface IOrdersRepository
	{
		Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
		Task<OrderViewModel> GetOrderByIdAsync(int orderId);
		Task AddOrderAsync(OrderViewModel orderViewModel);
		Task UpdateOrderAsync(OrderViewModel orderViewModel);
		Task DeleteOrderAsync(int orderId);
	}

}
