using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace ServicesGatway.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airplane> Airplanes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=IsraFlight_DB.mssql.somee.com;Database=IsraFlight_DB;User Id=moriya058_SQLLogin_1;Password=5ihvk3tmev;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airplane>(entity =>
        {
            entity.HasKey(e => e.AirplaneId).HasName("PK__Airplane__5ED76B858470A223");

            entity.ToTable("Airplane");

            entity.Property(e => e.AirplaneId).HasColumnName("AirplaneID");
            entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Nickname).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8D5F0CD00");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.IsManager).HasColumnType("bit");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flight__8A9E148EAB90B873");

            entity.ToTable("Flight", tb => tb.HasTrigger("trg_SetAvailableSeats"));

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.AirplaneId).HasColumnName("AirplaneID");
            entity.Property(e => e.ArrivalAirport).HasMaxLength(100);
            entity.Property(e => e.ArrivalCity).HasMaxLength(100);
            entity.Property(e => e.ArrivalCountry).HasMaxLength(100);
            entity.Property(e => e.ArrivalDateTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureAirport).HasMaxLength(100);
            entity.Property(e => e.DepartureCity).HasMaxLength(100);
            entity.Property(e => e.DepartureCountry).HasMaxLength(100);
            entity.Property(e => e.DepartureDateTime).HasColumnType("datetime");

            // הגדרת שדה המחיר
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627E69FB46E");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
