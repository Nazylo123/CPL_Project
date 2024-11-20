using ShoesStore.Model;

namespace ShoesStore.ViewModel.RequestModel
{
    public class ProductViewModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public List<int> SizeId { get; set; } 
		public List<string> SizeName { get; set; } 
		public List<int> Quantity { get; set; }
		public string Url { get; set; }
	}
}
