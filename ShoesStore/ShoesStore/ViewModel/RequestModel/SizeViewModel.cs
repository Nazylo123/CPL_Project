namespace ShoesStore.ViewModel.RequestModel
{
	public class SizeViewModel
	{
		public int Id { get; set; }  // Mã size
		public string SizeName { get; set; }  // Tên kích cỡ
		public List<int> ProductId { get; set; }  // Danh sách ProductSizeStocks
		public List<int> Quantity { get; set; }  // Danh sách ProductSizeStocks
		


	}
}
