using Axiom.Application.Interfaces.Service;
using Axiom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Axiom.Infrastructure.Contexts
{
    public class AxiomDBContext : DbContext
    {
        private ICurrentUserService _userService;
        public AxiomDBContext()
        {
        }

        public AxiomDBContext(DbContextOptions<AxiomDBContext> options, ICurrentUserService userService)
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
            optionsBuilder.UseSqlServer(_userService.ConnectionString);
        }

        public string GetCustomerKey()
        {
            throw new System.NotImplementedException();
        }

        public DbSet<EncounterForm> EncounterForms { get; set; }
        public DbSet<convFormSecurity> convFormSecurity { get; set; }
    }
}
