using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.Repository;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.ResponseModel;
namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmailRepository emailRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(
    UserManager<IdentityUser> userManager,
    ITokenRepository tokenRepository,
    IEmailRepository emailRepository) 
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.emailRepository = emailRepository; 
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel registerRequest)
        {
            // Tạo user mới
            var user = new IdentityUser { UserName = registerRequest.Email.Trim(), Email = registerRequest.Email.Trim() };

            var result = await userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(user, "CUSTOMER");
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationUrl = Url.Action("ConfirmEmail", "Auth", new
                    {
                        email = user.Email,
                        token = token,
                        
                    }, Request.Scheme);

                    string subject = "Xác nhận email đăng ký";
                    string body = $"Vui lòng nhấn vào đường link sau để xác nhận tài khoản của bạn: {confirmationUrl}";
                    await emailRepository.SendEmail(user.Email, subject, body);
                    Console.WriteLine(confirmationUrl);
                    return Ok($"Đăng ký thành công. Vui lòng kiểm tra email để xác nhận tài khoản.=>{confirmationUrl}");
                }
                else
                {
                    if (result.Errors.Any())
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            // Tìm user theo ID
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return BadRequest("User không tồn tại.");
            }

            // Xác nhận email
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Xác nhận email thành công. Bạn có thể đăng nhập.");
            }

            return BadRequest("Xác nhận email thất bại.");
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            

            if (identityUser is not null)
            {
                if (identityUser.EmailConfirmed == false)
                {
                    return BadRequest("Email chua duoc confirm");
                }

                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    
                    var roles = await userManager.GetRolesAsync(identityUser);

                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());


                    var response = new LoginResponseViewModel()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest("Email không tồn tại.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPasswordUrl = Url.Action("ResetPassword", "Auth", new
            {
                email = request.Email,
                token = token
            }, Request.Scheme);

            string subject = "Đặt lại mật khẩu";
            string body = $"Vui lòng nhấn vào đường link sau để đặt lại mật khẩu: <a href='{resetPasswordUrl}'>Đặt lại mật khẩu</a>";

            await emailRepository.SendEmail(request.Email, subject, body);

            return Ok($"Đã gửi email đặt lại mật khẩu. Vui lòng kiểm tra hộp thư.  token:{token}");
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest("Email không hợp lệ.");
            }

            var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return ValidationProblem(ModelState);
            }

            return Ok("Đặt lại mật khẩu thành công.");
        }


    }
}
    


