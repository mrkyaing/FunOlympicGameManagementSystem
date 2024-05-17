using FunOlympicGameManagementSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FunOlympicGameManagementSystem.Models.DAO {
    public class AppDbContext :DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
        }
        public DbSet<UserEntity> Users{ get; set; }
         public DbSet<WishListGameEntity> WishListGames { get; set; }
        public DbSet<OTPEntity> OTPs { get; set; }
        public DbSet<ContactEnquiryEntity> ContactEnquiries { get; set; }
        
    }
}
