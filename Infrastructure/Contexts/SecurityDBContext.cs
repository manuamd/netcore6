using Axiom.Application.Interfaces.Service;
using Axiom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Axiom.Infrastructure.Contexts
{
    public class SecurityDBContext : DbContext
    {
        ICurrentUserService _userService;
        public SecurityDBContext()
        {
        }
        public SecurityDBContext(DbContextOptions<SecurityDBContext> options, ICurrentUserService userService)
        : base(options)
        {
            _userService = userService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_userService.SecurityConnectionString);
        }

        public virtual DbSet<SecurityUser> SecurityUser { get; set; }
        public virtual DbSet<SecurityPassword_User> SecurityPassword_User { get; set; }
        public virtual DbSet<SecurityPassword> SecurityPassword { get; set; }
        public virtual DbSet<ConversionSecurityUserCustomer> ConversionSecurityUserCustomer { get; set; }
        public virtual DbSet<ConversionSecurityUserRole> ConversionSecurityUserRoles { get; set; }
    }
}
