using ShoesStore.ViewModel.RequestModel;
using WebApi.Models;

namespace ShoesStore.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserRequestViewModel>> GetAllUsersAsync();
        Task<UserRequestViewModel?> GetUserByEmailAsync(string email); 
        Task<UserRequestViewModel?> AddUserAsync(UserRequestViewModel request);
        Task<UserRequestViewModel?> UpdateUserAsync(string email, UserRequestViewModel request); 
        Task<bool> DeleteUserAsync(string email); 
    }
}
