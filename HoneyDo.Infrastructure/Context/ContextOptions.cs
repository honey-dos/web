using Microsoft.EntityFrameworkCore;

namespace HoneyDo.Infrastructure.Context
{
    public class ContextOptions<TContext> where TContext : DbContext
    {
        public string ConnectionString { get; set; }
    }
}
