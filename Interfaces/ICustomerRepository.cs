using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface ICustomerRepository
    {
        Task CreateAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<List<Customer>?> GetAllAsync();
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }
}
