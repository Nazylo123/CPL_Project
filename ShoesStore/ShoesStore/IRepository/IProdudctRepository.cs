using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.ResponseModel;
namespace ShoesStore.IRepository
{
    public interface IProdudctRepository
    {
        Task<IEnumerable<ProductReponseViewModel>> GetAllProductAsync();
        Task AddProductAsync(ProductRequestModel productRequest, int categoryId);
        Task<bool> DeleteProductAsync(int productId);
        Task<ProductReponseViewModel> GetProductAsync(int productId);
    }
}
