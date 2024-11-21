namespace ShoesStore.ViewModel.RequestModel
{
	public class UpdateStockRequest
	{
		public int ProductId { get; set; }
		public int SizeId { get; set; }
		public int Quantity { get; set; }
	}
}
