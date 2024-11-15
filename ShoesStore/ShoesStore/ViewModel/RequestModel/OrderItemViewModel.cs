namespace ShoesStore.ViewModel.RequestModel
{
	public class OrderItemViewModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int SizeId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string ProductName { get; set; }
		public string SizeName { get; set; }
	}
}
