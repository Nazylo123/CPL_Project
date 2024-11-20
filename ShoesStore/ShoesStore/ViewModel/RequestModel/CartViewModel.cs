namespace ShoesStore.ViewModel.RequestModel
{
    public class CartViewModel
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
