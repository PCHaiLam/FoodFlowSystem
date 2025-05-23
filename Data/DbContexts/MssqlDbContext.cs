﻿using FoodFlowSystem.Entities.AuditLog;
using FoodFlowSystem.Entities.Category;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.OAuth;
using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Entities.Product;
using FoodFlowSystem.Entities.ProductVersions;
using FoodFlowSystem.Entities.Reservation;
using FoodFlowSystem.Entities.Role;
using FoodFlowSystem.Entities.Table;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Data.DbContexts
{
    public class MssqlDbContext : DbContext
    {
        private readonly AuditLogInterceptor _auditLogInterceptor;
        private readonly TimeInterceptor _timeInterceptor;
        public MssqlDbContext(
            DbContextOptions<MssqlDbContext> options, 
            AuditLogInterceptor auditLogInterceptor,
            TimeInterceptor timeInterceptor) : base(options)
        {
            _auditLogInterceptor = auditLogInterceptor;
            _timeInterceptor = timeInterceptor;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<AuditLogEntity> AuditLogs { get; set; }
        public DbSet<PaymentEntity> Paymnents { get; set; }
        public DbSet<InvoiceEntity> Invoices { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TableEntity> Tables { get; set; }
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<OAuthEntity> OAuths { get; set; }
        public DbSet<ProductVersionEntity> ProductVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderItemConfig());
            modelBuilder.ApplyConfiguration(new AuditLogConfig());
            modelBuilder.ApplyConfiguration(new PaymentConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new TableConfig());
            modelBuilder.ApplyConfiguration(new FeedbackConfig());
            modelBuilder.ApplyConfiguration(new ReservationConfig());
            modelBuilder.ApplyConfiguration(new OAuthConfig());
            modelBuilder.ApplyConfiguration(new ProductVersionConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.AddInterceptors(_auditLogInterceptor);
            optionsBuilder.AddInterceptors(_timeInterceptor);
        }
    }
}
