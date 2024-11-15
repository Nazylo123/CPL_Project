using Microsoft.AspNetCore.Mvc;
using ShoesStore.ViewModel.RequestModel;
namespace ShoesStore.IRepository
{
    public interface IAccountRepository
    {
        String createAccount([FromBody] RegisterRequestViewModel registerRequest);
    }
}
