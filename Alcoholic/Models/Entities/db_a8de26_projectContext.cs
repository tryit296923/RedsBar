using System;
using System.Collections.Generic;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Alcoholic.Models.Entities
{
    public partial class db_a8de26_projectContext : DbContext
    {
        public db_a8de26_projectContext()
        {
        }

        public db_a8de26_projectContext(DbContextOptions<db_a8de26_projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MonthlyRecord> MonthlyRecords { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Reserves> Reserves { get; set; }
        public virtual DbSet<DeskInfo> DeskInfo { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<ProductsMaterials> ProductsMaterials { get; set; }

        protected override void OnModelCreating([FromBody] ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK_Employee_1");

                entity.ToTable("Employee");

                entity.Property(e => e.EmpId).HasColumnName("EmpID")
                .ValueGeneratedOnAdd();

                entity.Property(e => e.EmpAccount).HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmpName).HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmpPassword).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Salt).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Role).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK_Material_1");

                

                entity.Property(e => e.Name).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Brand).HasColumnType("nvarchar(max)");

                entity.HasOne(x=>x.Category).WithMany(x=>x.Materials).HasForeignKey(x => x.CategoryId);
                entity.HasMany(x => x.ProductsMaterials).WithOne(x => x.Material).HasForeignKey(x=>x.MaterialId);

                

            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberID).HasName("PK_Member_1");

                entity.Property(e => e.MemberID)
                    .HasColumnName("MemberID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Age).HasComputedColumnSql("(datediff(day,[MemberBirth],getdate())/(365))", false);

                entity.Property(e => e.Email).HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmailID)
                    .HasColumnName("EmailID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.MemberAccount).HasColumnType("nvarchar(max)");

                entity.Property(e => e.MemberBirth).HasColumnType("date");

                entity.Property(e => e.MemberName).HasColumnType("nvarchar(max)");

                entity.Property(e => e.MemberPassword).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Phone).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Qualified)
                    .HasMaxLength(1)
                    .HasColumnName("qualified")
                    .HasDefaultValueSql("(N'n')")
                    .IsFixedLength();
            });

            modelBuilder.Entity<MonthlyRecord>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.Month })
                    .HasName("pk_Time");

                entity.ToTable("MonthlyRecord");

                entity.Property(e => e.Year).HasColumnType("date");

                entity.Property(e => e.Month).HasColumnType("date");

                entity.Property(e => e.Percentage).HasColumnType("decimal");

                entity.Property(e => e.HotProduct1).HasColumnType("nvarchar(max)");
                entity.Property(e => e.HotProduct2).HasColumnType("nvarchar(max)");
                entity.Property(e => e.HotProduct3).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID");

                entity.Property(e => e.OrderTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Members");
                entity.HasOne(x => x.Feedback).WithOne(x => x.Order).HasForeignKey<Feedback>(x => x.OrderId);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.Property(e => e.Discount).HasColumnType("decimal");

                entity.Property(e => e.Rate).HasColumnType("nvarchar(max)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Products");

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK_Product_1");

                entity.Property(e => e.ProductName).HasColumnType("nvarchar(max)");
                entity.HasMany(x => x.OrderDetails).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
                entity.HasMany(x => x.ProductsMaterials).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
                entity.HasMany(x => x.Images).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<ProductsMaterials>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Material).WithMany(x => x.ProductsMaterials).HasForeignKey(x => x.MaterialId);
                entity.HasOne(x=>x.Product).WithMany(x => x.ProductsMaterials).HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<Reserves>(entity =>
            {
                entity.HasKey(e => e.ReserveId).HasName("PK_Reserves_1");

                entity.Property(e => e.ReserveId)
                .HasColumnName("ReserveID")
                .ValueGeneratedOnAdd();

                entity.Property(e => e.ReserveDate).HasColumnType("date");
                
                entity.Property(e => e.ReserveName).HasColumnType("nvarchar(max)");
                
                entity.Property(e => e.Phone).HasColumnType("nvarchar(max)");
                
                entity.Property(e => e.Email).HasColumnType("nvarchar(max)");

                entity.Property(e => e.ReserveSet).HasColumnType("date");
            });

            modelBuilder.Entity<DeskInfo>(entity =>
            {
                entity.HasKey(e => e.DeskId).HasName("PK_DeskInfo_1");

                entity.Property(e => e.DeskId).HasColumnType("int").ValueGeneratedOnAdd();

                entity.Property(e => e.StartTime).HasColumnType("nvarchar(max)");

                entity.Property(e => e.Number).HasColumnType("nvarchar(max)");

                entity.Property(e => e.EndTime).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.DiscountId).HasName("PK_Discount_1");

                entity.Property(e => e.DiscountId).ValueGeneratedOnAdd();

                entity.Property(e => e.DiscountName).HasColumnType("nvarchar(max)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal");

            });

            modelBuilder.Entity<Feedback>( entity =>
            {
                entity.HasKey(e=> e.OrderId);

                entity.Property(e => e.Age)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.FeedbackName)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.Suggestion)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                //entity.HasOne(f => f.Order)
                //    .WithOne(o => o.Feedback)
                //    .HasForeignKey<Order>(f => f.OrderId)
                //    .OnDelete(DeleteBehavior.Cascade);
            });

                OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
