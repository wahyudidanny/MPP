
using Microsoft.EntityFrameworkCore;
using MPP.API.Entities;

namespace MPP.API.Repository
{
    public class MPPDBContext : DbContext
    {
        public MPPDBContext(DbContextOptions<MPPDBContext> options) : base(options) { }
        public DbSet<T_MsMPP>? T_MsMPP { get; set; }
        public DbSet<T_MsMPPDetail>? T_MsMPPDetail { get; set; }

    }
}