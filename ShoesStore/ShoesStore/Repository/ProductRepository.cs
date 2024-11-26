
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;
using ShoesStore.Model;
using ShoesStore.ViewModel.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ShoesStore.Repository
    
{
    public class ProductRepository : IProdudctRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            try
            {
                var productsize = _context.ProductSizeStocks.Where(e => e.ProductId == product.Id);
                var productimage = _context.ProductImages.Where(e => e.ProductId == product.Id);
                foreach (var item in productsize)
                {
                    _context.ProductSizeStocks.Remove(item);

                }
                foreach (var item in productimage)
                {
                    _context.ProductImages.Remove(item);

                }
                _context.Remove(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        public async Task<bool> UpdateProductAsync([FromForm] ProductRequestModel productRequestModel, int id)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return false; // Return false if product not found
            }

            try
            {
                // Update product details
                existingProduct.Name = productRequestModel.Name;
                existingProduct.Description = productRequestModel.Description;
                existingProduct.Price = productRequestModel.Price;
                existingProduct.UpdatedAt = DateTime.Now;

                // Update product size stocks
                _context.ProductSizeStocks.RemoveRange(existingProduct.ProductSizeStocks); // Remove old sizes
                foreach (var sizeQuantity in productRequestModel.SizeQuantities)
                {
                    var sizeStock = new ProductSizeStock
                    {
                        ProductId = existingProduct.Id,
                        SizeId = sizeQuantity.SizeId,
                        Quantity = sizeQuantity.Quantity
                    };
                    _context.ProductSizeStocks.Add(sizeStock);
                }

                // Update product images
                _context.ProductImages.RemoveRange(existingProduct.ProductImages); // Remove old images
                foreach (var imageUrl in productRequestModel.ImageUrls)
                {
                    var productImage = new ProductImage
                    {
                        ProductId = existingProduct.Id,
                        ImageUrl = imageUrl,
                        IsPrimary = false // Set primary flag if needed
                    };
                    _context.ProductImages.Add(productImage);
                }

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLineAsync("Error: " + ex);
                return false;
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
