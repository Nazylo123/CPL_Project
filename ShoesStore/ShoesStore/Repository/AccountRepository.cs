using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        string IAccountRepository.createAccount(RegisterRequestViewModel registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
