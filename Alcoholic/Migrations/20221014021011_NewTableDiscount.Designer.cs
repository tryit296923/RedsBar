﻿// <auto-generated />
using System;
using Alcoholic.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Alcoholic.Migrations
{
    [DbContext(typeof(db_a8de26_projectContext))]

    [Migration("20221014021011_NewTableDiscount")]
    partial class NewTableDiscount


    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Alcoholic.Models.Entities.DeskInfo", b =>
                {
                    b.Property<string>("Desk")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EndTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupied")
                        .HasColumnType("int");

                    b.Property<string>("StartTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Desk");

                    b.ToTable("DeskInfo");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiscountId"), 1L, 1);

                    b.Property<float>("DiscountAmount")
                        .HasColumnType("real");

                    b.Property<string>("DiscountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Qualify")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DiscountId");

                    b.HasIndex("ProductId");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Employee", b =>
                {
                    b.Property<string>("EmpId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("EmpID");

                    b.Property<string>("EmpAccount")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EmpName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EmpPassword")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("EmpId")
                        .HasName("PK_Employee_1");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Material", b =>
                {
                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<short>("Cost")
                        .HasColumnType("smallint");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ShippingDate")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Member", b =>
                {
                    b.Property<string>("MemberID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("MemberID");

                    b.Property<int?>("Age")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("(datediff(day,[MemberBirth],getdate())/(365))", false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("EmailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("emailID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("MemberAccount")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("MemberBirth")
                        .HasColumnType("date");

                    b.Property<int>("MemberLevel")
                        .HasColumnType("int");

                    b.Property<string>("MemberName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MemberPassword")
                        .IsRequired()
                        .HasMaxLength(225)
                        .HasColumnType("nvarchar(225)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Qualified")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasColumnName("qualified")
                        .HasDefaultValueSql("(N'n')")
                        .IsFixedLength();

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MemberID");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.MonthlyRecord", b =>
                {
                    b.Property<DateTime>("Year")
                        .HasColumnType("date");

                    b.Property<DateTime>("Month")
                        .HasColumnType("date");

                    b.Property<int>("CustNumber")
                        .HasColumnType("int");

                    b.Property<string>("HotProduct1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HotProduct2")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HotProduct3")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<int>("Quantity1")
                        .HasColumnType("int");

                    b.Property<int>("Quantity2")
                        .HasColumnType("int");

                    b.Property<int>("Quantity3")
                        .HasColumnType("int");

                    b.HasKey("Year", "Month")
                        .HasName("pk_Time");

                    b.ToTable("MonthlyRecord", (string)null);
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("OrderID");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("MemberID");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("smalldatetime");

                    b.HasKey("OrderId");

                    b.HasIndex("MemberId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.OrderDetail", b =>
                {
                    b.Property<string>("OrderId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("OrderID");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("ProductID");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Rate")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Sum")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProductID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<int?>("Cointreau")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int?>("Gin")
                        .HasColumnType("int");

                    b.Property<string>("ImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LemonNade")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Rum")
                        .HasColumnType("int");

                    b.Property<int>("SaleAt")
                        .HasColumnType("int");

                    b.Property<int?>("Sugar")
                        .HasColumnType("int");

                    b.Property<int?>("Tequila")
                        .HasColumnType("int");

                    b.Property<int>("UnitPrice")
                        .HasColumnType("int");

                    b.Property<int?>("Vermouth")
                        .HasColumnType("int");

                    b.Property<int?>("Vodka")
                        .HasColumnType("int");

                    b.Property<int?>("Wiskey")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Reserves", b =>
                {
                    b.Property<int>("ReserveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ReserveID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReserveId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("ReserveDate")
                        .HasColumnType("date");

                    b.Property<string>("ReserveName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("ReserveSet")
                        .HasColumnType("date");

                    b.HasKey("ReserveId")
                        .HasName("PK_Reserves_1");

                    b.ToTable("Reserves");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Discount", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Order", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Member", "Member")
                        .WithMany("Orders")
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Members");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.OrderDetail", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Order");

                    b.HasOne("Alcoholic.Models.Entities.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Products");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Member", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Product", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}