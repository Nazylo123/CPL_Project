using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.Model.Momo;
using ShoesStore.Services;

namespace ShoesStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MomoController : ControllerBase
	{

		private readonly IMomoService _momoService;
		public MomoController(IMomoService momoService)
		{
			_momoService = momoService;
		}

		[HttpPost("create-payment")]
		public async Task<IActionResult> CreatePayment([FromBody] MomoExecuteResponseModel model)
		{
			if (model == null)
			{
				return BadRequest(new
				{
					Message = "Dữ liệu đầu vào không hợp lệ.",
					ErrorCode = 400
				});
			}

			if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Amount) || string.IsNullOrEmpty(model.OrderInfo))
			{
				return BadRequest(new
				{
					Message = "Thiếu thông tin bắt buộc: FullName, Amount hoặc OrderInfo.",
					ErrorCode = 400
				});
			}

			try
			{
				var result = await _momoService.CreatePaymentAsync(model);

				if (result == null || result.ErrorCode != 0)
				{
					return StatusCode(500, new
					{
						Message = $"Tạo thanh toán thất bại: {result?.Message ?? "Không xác định"}",
						ErrorCode = result?.ErrorCode ?? 500
					});
				}

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					Message = "Có lỗi xảy ra khi tạo thanh toán.",
					Details = ex.Message,
					ErrorCode = 500
				});
			}
		}
		[HttpGet("payment-result")]
		public IActionResult PaymentResult()
		{
			try
			{
				// Lấy các tham số từ URL query string
				var queryParams = HttpContext.Request.Query;

				// Gọi dịch vụ để xử lý và trả về kết quả
				var result = _momoService.PaymentExecuteAsync(queryParams);

				// Trả về kết quả nếu mọi thứ hợp lệ
				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				// Trả về lỗi nếu thiếu tham số bắt buộc
				return BadRequest(new
				{
					Message = ex.Message,
					ErrorCode = 400
				});
			}
			catch (Exception ex)
			{
				// Trả về lỗi chung nếu có lỗi khác xảy ra
				return StatusCode(500, new
				{
					Message = "Có lỗi xảy ra khi xử lý kết quả thanh toán.",
					Details = ex.Message,
					ErrorCode = 500
				});
			}
		}


	}

}
