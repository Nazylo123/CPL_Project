using ShoesStore.Model;

namespace ShoesStore.ViewModel.RequestModel
{
    public class ProductRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; } // Cần thêm CategoryId nếu chưa có
        public List<SizeQuantityModel> SizeQuantities { get; set; } // Danh sách chứa SizeId và Quantity
        public List<string> ImageUrls { get; set; } // Danh sách URL ảnh sản phẩm
    }

    public class SizeQuantityModel
    {
        public int SizeId { get; set; }
        public int Quantity { get; set; }
    }

}
