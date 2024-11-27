using ShoesStore.Model;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.ViewModel.ResponseModel
{
    public class ProductReponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<SizeQuantityModel> SizeQuantities { get; set; }
    }
}
