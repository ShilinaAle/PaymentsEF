using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentsEF.Model
{
    public partial class PaymentsEFContext : DbContext
    {
        public PaymentsEFContext()
        {
        }

        public PaymentsEFContext(DbContextOptions<PaymentsEFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arrivals> Arrivals { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-O49ALTT\\SQLEXPRESS01;Initial Catalog=PaymentsDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arrivals>(entity =>
            {
                entity.HasKey(e => e.Idarrival)
                    .HasName("PK__Arrivals__6CE8374291A8DE12");

                entity.Property(e => e.Idarrival).HasColumnName("IDArrival");

                entity.Property(e => e.ArrivalDate).HasColumnType("datetime");

                entity.Property(e => e.Remains)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SumOfArrival).HasColumnType("money");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.Idorder)
                    .HasName("PK__Orders__5CBBCADBDCC27E24");

                entity.Property(e => e.Idorder).HasColumnName("IDOrder");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Payment).HasColumnType("money");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Payments>(entity =>
            {

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ArrivalId).HasColumnName("ArrivalID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Arrival)
                    .WithMany()
                    .HasForeignKey(d => d.ArrivalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payments__Arriva__5070F446");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payments__OrderI__4F7CD00D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
