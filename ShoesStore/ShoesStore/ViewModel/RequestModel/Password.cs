namespace ShoesStore.ViewModel.RequestModel
{
    public class Password
    {
    }
    public class ForgotPasswordRequestModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequestModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}
