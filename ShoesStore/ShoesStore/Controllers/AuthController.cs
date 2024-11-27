using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ShoesStore.IRepository;
using ShoesStore.Repository;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.ResponseModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebApi.Models;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IEmailRepository emailRepository;
		private readonly UserManager<AppUser> userManager;
		private readonly ITokenRepository tokenRepository;

		private string token1;

		public AuthController(
			UserManager<AppUser> userManager,
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
			var user = new AppUser { UserName = registerRequest.Email.Trim(), Email = registerRequest.Email.Trim() };

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

					return Ok($"Đăng ký thành công. Vui lòng kiểm tra email để xác nhận tài khoản. => {confirmationUrl}");
				}
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return ValidationProblem(ModelState);
		}

		[HttpGet]
		[Route("confirm-email")]
		public async Task<IActionResult> ConfirmEmail(string email, string token)
		{
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return BadRequest("User không tồn tại.");
			}
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
			if (identityUser != null)
			{
				if (!identityUser.EmailConfirmed)
				{
					return BadRequest("Email chưa được xác nhận.");
				}

				if (await userManager.CheckPasswordAsync(identityUser, request.Password))
				{
					var roles = await userManager.GetRolesAsync(identityUser);
					var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

					return Ok(new LoginResponseViewModel
					{
						Email = request.Email,
						Roles = roles.ToList(),
						Token = jwtToken
					});
				}
			}

			ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
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
			token1 = token;
			var encodedToken = HttpUtility.UrlEncode(token);
			var resetPasswordUrl = $"http://localhost:4200/reset-password/{encodedToken}/{HttpUtility.UrlEncode(request.Email)}";

			await emailRepository.SendEmail(request.Email, "Đặt lại mật khẩu",
				$"Vui lòng nhấn vào link sau để đặt lại mật khẩu: <a href='{resetPasswordUrl}'>Đặt lại mật khẩu</a>");

			return Ok($"Nếu email tồn tại, đường dẫn đặt lại mật khẩu sẽ được gửi.{encodedToken}");
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

			var result = await userManager.ResetPasswordAsync(user, DecodeToken(request.Token), request.NewPassword);
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

		private string DecodeToken(string token)
		{
			// Sử dụng regex để thay thế tất cả các ký tự không phải là chữ cái, số và dấu gạch dưới thành "%"
			//string decodedToken = Regex.Replace(token, @"[^a-zA-Z0-9_-]", "%");

			// Giải mã URL sau khi thay thế
			return HttpUtility.UrlDecode(token);
		}

		[HttpPost]
		[Route("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVm changePasswordVm)
		{
			var user = await userManager.FindByEmailAsync(changePasswordVm.Email);
			if (user == null)
			{
				return BadRequest("Email không hợp lệ.");
			}

			if (!await userManager.CheckPasswordAsync(user, changePasswordVm.Password))
			{
				return BadRequest("Mật khẩu cũ không đúng.");
			}

			var result = await userManager.ChangePasswordAsync(user, changePasswordVm.Password, changePasswordVm.NewPassword);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return ValidationProblem(ModelState);
			}

			return Ok("Mật khẩu đã được thay đổi thành công.");
		}
	}
}