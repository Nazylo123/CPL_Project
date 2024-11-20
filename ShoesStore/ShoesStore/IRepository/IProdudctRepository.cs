using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.ViewModel.ResponseModel;
namespace ShoesStore.IRepository
{
    public interface IProdudctRepository
    {
        Task<IEnumerable<ProductReponseViewModel>> GetAllProductAsync();
        public Task AddProductAsync(ProductRequestModel productRequest, int categoryId);
        Task<bool> DeleteproductAsync(int productId);
        Task<ProductReponseViewModel> GetProductAsync(int productId);
        Task<bool> UpdateProdudctAsync(ProductRequestModel productRequestModel, int productId);
    }
}
