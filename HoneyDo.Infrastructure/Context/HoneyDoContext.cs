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
        public DbSet<GroupAccount> GroupAccounts { get; set; }

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
                a.Ignore(i => i.Groups);
                a.Ignore(i => i.Tasks);
            });
            modelBuilder.Entity<GroupAccount>(groupAccount =>
            {
                groupAccount.HasKey(g => new { g.GroupId, g.AccountId });
                groupAccount.HasOne<Group>()
                    .WithMany("_groupAccounts")
                    .HasForeignKey(g => g.GroupId);
                groupAccount.HasOne<Account>()
                    .WithMany("_groupAccounts")
                    .HasForeignKey(g => g.AccountId);
            });
            modelBuilder.Entity<Group>(group =>
            {
                group.Ignore(i => i.Accounts);
                group.Ignore(i => i.Tasks);
                group.HasMany<Todo>("_tasks")
                    .WithOne()
                    .HasForeignKey(to => to.GroupId);
            });
        }
    }
}
