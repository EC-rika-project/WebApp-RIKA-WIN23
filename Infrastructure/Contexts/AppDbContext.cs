//using Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Contexts
//{
//    public class AppDbContext : DbContext
//    {
//        public DbSet<OrderEntity> Orders { get; set; }
//        public DbSet<OrderItem> OrderItems { get; set; }

//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<OrderEntity>()
//                .HasMany(o => o.Items)
//                .WithOne(i => i.Orders)
//                .HasForeignKey(i => i.OrderId);
//        }
//    }

//}
