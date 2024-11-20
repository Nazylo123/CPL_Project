
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;
using ShoesStore.Model;
using ShoesStore.ViewModel.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace ShoesStore.Repository
    
{
    public class ProductRepository : IProdudctRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteproductAsync(int productId)
        {
            var product =await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }
            else
            {
                _context.Products.Remove(product);
                _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<ProductReponseViewModel>> GetAllProductAsync()
        {
            var products = await _context.Products
                .Include(ps => ps.ProductSizeStocks)
                .Include(pi => pi.ProductImages)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync();

            return products.Select(product =>
            {
                var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);

                return new ProductReponseViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryName = category?.Name,
                    Price = product.Price,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    ProductImages = product.ProductImages.Select(pi => new ProductImage
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageUrl,
                    }).ToList(),
                };
            }).ToList();
        }


        public async Task<ProductReponseViewModel> GetProductAsync(int productId)
        {
            var product = await _context.Products
                .Include(ps => ps.ProductSizeStocks)
                .Include(pi => pi.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);

            var category = await _context.Categories
                                        .Where(c => c.Id == product.CategoryId)
                                        .Select(c => new CategoryViewModel
                                        {
                                            Name = c.Name,
                                            Description = c.Description,
                                            ImageUrl = c.ImageUrl
                                        })
                                        .FirstOrDefaultAsync();
            return new ProductReponseViewModel
            {
                Name = product.Name,
                Description = product.Description,
                CategoryName = category.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                ProductImages = product.ProductImages.Select(pi => new ProductImage
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl,

                }).ToList(),
                

            };
        }

        public async Task<bool> UpdateProdudctAsync(ProductRequestModel productRequestModel, int productId)
        {
            
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (existingProduct == null)
            {
                return false;
            }

            else
            {
                existingProduct.Name = productRequestModel.Name;
                existingProduct.Description = productRequestModel.Description;
                existingProduct.Price = productRequestModel.Price;
                existingProduct.UpdatedAt = DateTime.Now;


                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return true;
            }
        }


        Task IProdudctRepository.AddProductAsync(ProductRequestModel productRequest, int categoryId)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = categoryId,
                CreatedAt = DateTime.Now,
            };
            _context.Products.Add(product);
            return _context.SaveChangesAsync();
        }
    }
}
