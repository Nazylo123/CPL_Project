namespace ShoesStore.ViewModel.RequestModel
{
	public class OrderViewModel
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; }
		public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

	}
}
