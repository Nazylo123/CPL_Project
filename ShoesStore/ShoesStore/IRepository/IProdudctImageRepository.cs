using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.IRepository
{
    public interface IProdudctImageRepository
    {
        Task<IEnumerable<ProductImageRequestModel>> GetAllProductImageAsync(int productID);
        Task<ProductImageRequestModel> GetProductImageByIdAsync(int imageID);

        Task<ProductImageRequestModel> AddImageAsync(int productID,ProductImageRequestModel productImageRequestModel);

        Task<ProductImageRequestModel> UpdateImageAsync(int imageID, ProductImageRequestModel productImageRequestModel);    
        Task<bool> DeleteImageAsync(int imageID);
       
    }
}
