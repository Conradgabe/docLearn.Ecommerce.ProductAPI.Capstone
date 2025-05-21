using EcommerceSharedLibrary.Logs;
using EcommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.Repositories
{
    internal class ProductRepository(ProductDBContext context) : IProduct
    {
        private readonly ProductDBContext _context = context;
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));

                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");

                var currentProduct = _context.Products.Add(entity).Entity;
                await _context.SaveChangesAsync();

                if (currentProduct is not null && currentProduct.Id > 0)
                    return new Response(true, $"{entity.Name} added successfully");
                else
                    return new Response(false, $"Error occurred while adding {entity.Name}");

            } catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error Occurred adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);

                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                _context.Entry(product).State = EntityState.Detached;
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred deleting new product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error Occurred retrieving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error Occurred retrieving products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error Occurred retrieving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);

                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                _context.Entry(product).State = EntityState.Detached;
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();

                return new Response(true, $"{entity.Name} updated successfully");
            }
            catch (Exception ex)
{
                LogException.LogExceptions(ex);

                return new Response(false, "Error Occurred updating product");
            }
        }
    }
}
