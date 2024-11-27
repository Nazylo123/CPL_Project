using ShoesStore.Model;

namespace ShoesStore.ViewModel.RequestModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Kiểu string cho trạng thái
        public List<OrderItemViewModel> OrderItems { get; set; }
    }

}
