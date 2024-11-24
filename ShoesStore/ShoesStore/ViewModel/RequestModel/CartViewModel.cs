namespace ShoesStore.ViewModel.RequestModel
{
	using System.Text.Json.Serialization;

	public class CartViewModel
	{
		[JsonPropertyName("productId")]
		public int ProductId { get; set; }

		[JsonPropertyName("sizeId")]
		public int SizeId { get; set; }

		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }

		[JsonPropertyName("price")]
		public decimal Price { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("sizeName")]
		public string SizeName { get; set; }

		[JsonPropertyName("nameProdcut")]
		public string NameProduct { get; set; }
	}

	

}
