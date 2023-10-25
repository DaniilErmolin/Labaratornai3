using System;
using System.Collections.Generic;
using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3;

public partial class TouristAgency1Context : DbContext
{
    public TouristAgency1Context()
    {
    }

    public TouristAgency1Context(DbContextOptions<TouristAgency1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalService> AdditionalServices { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<TypesOfRecreation> TypesOfRecreations { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RC1TE3C;Initial Catalog=TouristAgency1;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC07BC28C93C");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC0709F595A5");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Fio)
                .HasMaxLength(150)
                .HasColumnName("FIO");
            entity.Property(e => e.Series).HasMaxLength(50);
            entity.Property(e => e.Sex).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC077EB2F5F7");

            entity.Property(e => e.Fio)
                .HasMaxLength(150)
                .HasColumnName("FIO");
            entity.Property(e => e.JobTitle).HasMaxLength(50);
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hotels__3214EC07F9846FAA");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.TheContactPerson).HasMaxLength(100);
        });

        modelBuilder.Entity<TypesOfRecreation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypesOfR__3214EC07C89167FD");

            entity.ToTable("TypesOfRecreation");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Restrictions).HasMaxLength(50);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vouchers__3214EC07A43B24A5");

            entity.Property(e => e.ExpirationDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.AdditionalService).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.AdditionalServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vouchers_AdditionalServices");

            entity.HasOne(d => d.Client).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vouchers_Clients");

            entity.HasOne(d => d.Employess).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.EmployessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vouchers_Employees");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vouchers_Hotels");

            entity.HasOne(d => d.TypeOfRecreation).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.TypeOfRecreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vouchers_TypesOfRecreation");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
