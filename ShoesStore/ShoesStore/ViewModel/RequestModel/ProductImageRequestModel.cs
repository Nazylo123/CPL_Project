using ShoesStore.Model;

namespace ShoesStore.ViewModel.RequestModel
{
    public class ProductImageRequestModel
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } 
    }
}
