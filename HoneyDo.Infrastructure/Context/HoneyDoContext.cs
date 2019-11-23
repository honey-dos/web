using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HoneyDo.Infrastructure.Context
{
    public class HoneyDoContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Group> Groups { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Login> Logins { get; set; }

        public HoneyDoContext(IOptions<ContextOptions<HoneyDoContext>> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(a =>
            {
                a.HasIndex(i => i.NormalizedUserName)
                    .IsUnique();
            });
            modelBuilder.Entity<Todo>(t =>
            {
                t.HasOne(to => to.Group)
                  .WithMany("Todos")
                  .HasForeignKey(to => to.GroupId);
            });
            /* modelBuilder.Entity<Group>(grp => */
            /* { */
            /*     grp.HasMany(g => g.Todos).WithOne(); */
            /*     grp.Ignore(g => g.Foobar); */
            /* }); */
        }
    }
}
