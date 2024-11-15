namespace ShoesStore.ViewModel.ResponseModel
{
    public class LoginResponseViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
