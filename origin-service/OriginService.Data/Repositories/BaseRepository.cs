using Microsoft.Extensions.Configuration;
using Npgsql;

namespace OriginService.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected abstract IConfiguration Configuration { get; set; }
        
        public NpgsqlConnection GetConnectionString()
        {
            return new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection"));
        }

    }
}