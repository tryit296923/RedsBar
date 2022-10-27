﻿// <auto-generated />
using System;
using Alcoholic.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Alcoholic.Migrations
{
    [DbContext(typeof(db_a8de26_projectContext))]
    [Migration("20221027114056_newEmpColumnStatusJoinReserveColumnStatus")]
    partial class newEmpColumnStatusJoinReserveColumnStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Alcoholic.Models.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.DeskInfo", b =>
                {
                    b.Property<int>("DeskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeskId"), 1L, 1);

                    b.Property<string>("Desk")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeskType")
                        .HasColumnType("int");

                    b.Property<string>("EndTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Occupied")
                        .HasColumnType("int");

                    b.Property<string>("StartTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeskId")
                        .HasName("PK_DeskInfo_1");

                    b.ToTable("DeskInfo");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiscountId"), 1L, 1);

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(38,17)");

                    b.Property<string>("DiscountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DiscountId")
                        .HasName("PK_Discount_1");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Employee", b =>
                {
                    b.Property<Guid>("EmpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmpID");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Join")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2022, 10, 27, 19, 40, 56, 562, DateTimeKind.Local).AddTicks(1954));

                    b.Property<DateTime?>("Leave")
                        .HasColumnType("datetime2");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Salary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(30000);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("EmpId")
                        .HasName("PK_Employee_1");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Feedback", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Age")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Dish")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Environment")
                        .HasColumnType("int");

                    b.Property<string>("FeedbackName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Frequency")
                        .HasColumnType("int");

                    b.Property<int?>("Overall")
                        .HasColumnType("int");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("Serve")
                        .HasColumnType("int");

                    b.Property<string>("Suggestion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaterialId"), 1L, 1);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<short>("Cost")
                        .HasColumnType("smallint");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MaterialId")
                        .HasName("PK_Material_1");

                    b.HasIndex("CategoryId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Member", b =>
                {
                    b.Property<Guid>("MemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MemberID");

                    b.Property<int?>("Age")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("(datediff(day,[MemberBirth],getdate())/(365))", false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EmailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmailID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("MemberAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MemberBirth")
                        .HasColumnType("date");

                    b.Property<int>("MemberLevel")
                        .HasColumnType("int");

                    b.Property<string>("MemberName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.HasKey("MemberID")
                        .HasName("PK_Member_1");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotProduct2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotProduct3")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(38,17)");

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeskNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MemberID");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Total")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("MemberId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.OrderDetail", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(38,17)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Rate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitPrice")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "Sequence", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitPrice")
                        .HasColumnType("int");

                    b.HasKey("ProductId")
                        .HasName("PK_Product_1");

                    b.HasIndex("DiscountId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.ProductsMaterials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Consumption")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsMaterials");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Reserves", b =>
                {
                    b.Property<int>("ReserveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ReserveID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReserveId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReserveDate")
                        .HasColumnType("date");

                    b.Property<string>("ReserveName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReserveSet")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("ReserveId")
                        .HasName("PK_Reserves_1");

                    b.ToTable("Reserves");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Feedback", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Order", "Order")
                        .WithOne("Feedback")
                        .HasForeignKey("Alcoholic.Models.Entities.Feedback", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Material", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Category", "Category")
                        .WithMany("Materials")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
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

            modelBuilder.Entity("Alcoholic.Models.Entities.Product", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Discount", "Discount")
                        .WithMany("Products")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.ProductImage", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.ProductsMaterials", b =>
                {
                    b.HasOne("Alcoholic.Models.Entities.Material", "Material")
                        .WithMany("ProductsMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alcoholic.Models.Entities.Product", "Product")
                        .WithMany("ProductsMaterials")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Category", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Discount", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Material", b =>
                {
                    b.Navigation("ProductsMaterials");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Member", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Order", b =>
                {
                    b.Navigation("Feedback")
                        .IsRequired();

                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Alcoholic.Models.Entities.Product", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("OrderDetails");

                    b.Navigation("ProductsMaterials");
                });
#pragma warning restore 612, 618
        }
    }
}
