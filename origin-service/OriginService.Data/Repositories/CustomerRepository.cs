using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OriginService.Data.Entities;
using Dapper;
using OriginService.Data.Repositories.Interfaces;


namespace OriginService.Data.Repositories
{
    public class CustomerRepository: BaseRepository, ICustomerRepository
    {
        protected override IConfiguration Configuration { get; set; }

        public CustomerRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<int> CreateCustomer(Customer customer)
        {
            var sqlCmd = "INSERT INTO originsvc.customer(first_name, last_name, email) VALUES(@firstName, @lastName, @email) RETURNING id";

            using (var conn = GetConnectionString())
            {
                conn.Open();

                var customerId = await conn.QueryFirstAsync<int>(sqlCmd,
                    new
                    {
                        firstName = customer.FirstName,
                        lastName = customer.LastName,
                        email = customer.Email
                    },
                    commandType: CommandType.Text);

                return customerId;

            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var sqlCmd = "SELECT * FROM originsvc.customer WHERE id = @id";
            
            using (var conn = GetConnectionString())
            {
                conn.Open();

                var customer = await conn.QueryFirstOrDefaultAsync(sqlCmd,
                    new {
                        id 
                    },
                    commandType: CommandType.Text);

                return customer == null ? null : new Customer { Id = customer.id,
                    FirstName = customer.first_name,
                    LastName = customer.last_name,
                    Email = customer.email
                    
                };
            }
        }
        
        public async Task UpdateCustomer(int id, Customer customer)
        {
            var sqlCmd = "UPDATE originsvc.customer SET first_name = @firstName, last_name = @lastName, email = @email WHERE id = @id";
            
            using (var conn = GetConnectionString())
            {
                conn.Open();

                await conn.ExecuteAsync(sqlCmd,
                    new {
                        id,
                        firstName = customer.FirstName,
                        lastName = customer.LastName,
                        email = customer.Email
                    },
                    commandType: CommandType.Text);
            }

        }

        public async Task DeleteCustomer(int id)
        {
            var sqlCmd = "DELETE FROM originsvc.customer WHERE id = @id";
            
            using (var conn = GetConnectionString())
            {
                conn.Open();

                await conn.ExecuteAsync(sqlCmd,
                    new {
                        id
                    },
                    commandType: CommandType.Text);
            }
        }
    }
}