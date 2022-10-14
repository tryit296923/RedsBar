using System;
using System.Collections.Generic;
using System.Security.Policy;
using Alcoholic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Alcoholic.Models.DTO
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

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MonthlyRecord> MonthlyRecords { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Reserves> Reserves { get; set; } = null!;
        public virtual DbSet<DeskInfo> DeskInfo { get; set; } = null!;
        public virtual DbSet<Discount> Discount { get; set; } = null!;



        protected override void OnModelCreating([FromBody] ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK_Employee_1");

                entity.ToTable("Employee");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(20)
                    .HasColumnName("EmpID");

                entity.Property(e => e.EmpAccount).HasMaxLength(20);

                entity.Property(e => e.EmpName).HasMaxLength(20);

                entity.Property(e => e.EmpPassword).HasMaxLength(20);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Brand).HasMaxLength(30);

                entity.Property(e => e.MaterialName).HasMaxLength(30);

                entity.Property(e => e.ShippingDate).HasMaxLength(10);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberID)
                    .HasMaxLength(20)
                    .HasColumnName("MemberID");

                entity.Property(e => e.Age).HasComputedColumnSql("(datediff(day,[MemberBirth],getdate())/(365))", false);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailID)
                    .HasColumnName("emailID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.MemberAccount).HasMaxLength(20);

                entity.Property(e => e.MemberBirth).HasColumnType("date");

                entity.Property(e => e.MemberName).HasMaxLength(20);

                entity.Property(e => e.MemberPassword).HasMaxLength(225);

                entity.Property(e => e.Phone).HasMaxLength(20);

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

                entity.Property(e => e.HotProduct1).HasMaxLength(50);

                entity.Property(e => e.HotProduct2).HasMaxLength(50);

                entity.Property(e => e.HotProduct3).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderID");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(20)
                    .HasColumnName("MemberID");

                entity.Property(e => e.OrderTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Members");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Rate).HasMaxLength(50);

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
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<Reserves>(entity =>
            {
                entity.HasKey(e => e.ReserveId)
                    .HasName("PK_Reserves_1");

                entity.Property(e => e.ReserveId).HasColumnName("ReserveID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.ReserveDate).HasColumnType("date");

                entity.Property(e => e.ReserveName).HasMaxLength(40);

                entity.Property(e => e.ReserveSet).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
