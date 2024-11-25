namespace ShoesStore.ViewModel.RequestModel
{
    public class CheckoutRequest
	{
		public List<CartViewModel> Cart { get; set; }
		public string Email { get; set; } = string.Empty;
	}
}
