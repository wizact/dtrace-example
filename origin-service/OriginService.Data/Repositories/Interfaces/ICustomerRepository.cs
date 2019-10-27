using System.Threading.Tasks;
using OriginService.Data.Entities;

namespace OriginService.Data.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> CreateCustomer(Customer customer);
        Task<Customer> GetCustomerById(int id);
        Task UpdateCustomer(int id, Customer customer);
        Task DeleteCustomer(int id);
    }
}