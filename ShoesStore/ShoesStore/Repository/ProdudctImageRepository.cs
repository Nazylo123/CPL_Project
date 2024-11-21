using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.Model;
using WebApi.Data;
namespace ShoesStore.Repository
{
    public class ProdudctImageRepository : IProdudctImageRepository
    {
        private readonly AppDbContext _context;

        public ProdudctImageRepository(AppDbContext context)
        {
            _context = context;
        }
        async Task<ProductImageRequestModel> IProdudctImageRepository.AddImageAsync(int productID, ProductImageRequestModel productImageRequestModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productID);

            if (product == null)
            {
                return null;
            }
            else
            {
                var newImage = new ProductImage
                {
                    ImageUrl = productImageRequestModel.ImageUrl,
                    ProductId = productID,
                    IsPrimary = productImageRequestModel.IsPrimary,

                };
                _context.ProductImages.Add(newImage);
                _context.SaveChangesAsync();
                return new ProductImageRequestModel
                {
                    IsPrimary = true,
                    ProductId = productID,
                    ImageUrl = productImageRequestModel.ImageUrl,
                };

            }
        }

        async Task<bool> IProdudctImageRepository.DeleteImageAsync(int imageID)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Id == imageID);
            if (image == null)
            {
                return false;
            }
            else
            {
                _context.ProductImages.Remove(image);
                _context.SaveChangesAsync();
                return true;
            }
        }

        async Task<IEnumerable<ProductImageRequestModel>> IProdudctImageRepository.GetAllProductImageAsync(int productID)
        {
            var images = await _context.ProductImages
                .Where(pi => pi.ProductId == productID)
                .Select(pi => new ProductImageRequestModel
                {
                    ProductId = productID,
                    ImageUrl = pi.ImageUrl,
                    IsPrimary = pi.IsPrimary,
                }).ToListAsync();

            return images;

        }
        public async Task<ProductImage> UpdateImageByProductIdAsync(int productID, ProductImageRequestModel productImageRequestModel)
        {
            var imageToUpdate = await _context.ProductImages
                                                 .FirstOrDefaultAsync(img => img.ProductId == productID);

            if (imageToUpdate == null)
            {
                return null;
            }


            imageToUpdate.ImageUrl = productImageRequestModel.ImageUrl;
            imageToUpdate.IsPrimary = productImageRequestModel.IsPrimary;
            await _context.SaveChangesAsync();

            return imageToUpdate;
        }

        async Task<ProductImageRequestModel> IProdudctImageRepository.GetProductImageByIdAsync(int imageID)
        {
            var image = await _context.ProductImages.
                FirstOrDefaultAsync(pi => pi.Id == imageID);

            return new ProductImageRequestModel
            {
                ImageUrl = image.ImageUrl,
                IsPrimary = image.IsPrimary,
            };
        }

        async Task<ProductImageRequestModel> IProdudctImageRepository.UpdateImageAsync(int imageID, ProductImageRequestModel productImageRequestModel)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == imageID);

            if (image != null)
            {
                return null;
            }

            else
            {
                image.ImageUrl = productImageRequestModel.ImageUrl;
                image.IsPrimary = productImageRequestModel.IsPrimary;
                await _context.SaveChangesAsync();
                return new ProductImageRequestModel
                {
                    ImageUrl = productImageRequestModel.ImageUrl,
                    IsPrimary = productImageRequestModel.IsPrimary,
                };
            }



        }

    }
}
