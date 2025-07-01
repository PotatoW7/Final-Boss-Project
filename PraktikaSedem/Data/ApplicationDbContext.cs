using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PraktikaSedem.Models;

namespace PraktikaSedem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PraktikaSedem.Models.Patient> Patient { get; set; } = default!;
        public DbSet<PraktikaSedem.Models.Doctor> Doctor { get; set; } = default!;
        public DbSet<PraktikaSedem.Models.Appointment> Appointment { get; set; } = default!;
    }
}
