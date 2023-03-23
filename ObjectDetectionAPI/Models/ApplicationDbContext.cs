using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ObjectDetectionAPI.Models.Image;

namespace ObjectDetectionAPI.Models
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public DbSet<FileStore> FileStores { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "Admin"
                },
                new IdentityRole(){
                Name = "User",
                    ConcurrencyStamp = "2",
                    NormalizedName = "User"
                }
                );
        }
    }
}
