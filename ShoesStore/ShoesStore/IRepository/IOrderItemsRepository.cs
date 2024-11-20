using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.IRepository
{
	public interface IOrderItemsRepository
	{
		Task<IEnumerable<OrderItemViewModel>> GetAllOrderItemsAsync();
		Task<OrderItemViewModel> GetOrderItemByIdAsync(int orderItemId);
		Task AddOrderItemAsync(OrderItemViewModel orderItemViewModel);
		Task UpdateOrderItemAsync(OrderItemViewModel orderItemViewModel);
		Task DeleteOrderItemAsync(int orderItemId);
	}
}
