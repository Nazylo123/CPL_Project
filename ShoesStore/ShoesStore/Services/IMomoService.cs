using ShoesStore.Model.Momo;

namespace ShoesStore.Services
{
	public interface IMomoService
	{
		public Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(MomoExecuteResponseModel model);

		public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}

