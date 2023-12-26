using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AirlinesApp.Models;

public partial class AirlinesContext : DbContext
{
    public AirlinesContext()
    {
    }

    public AirlinesContext(DbContextOptions<AirlinesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Airports_Cities");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.HasOne(d => d.AirportFrom).WithMany(p => p.FlightAirportFroms)
                .HasForeignKey(d => d.AirportFromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_Airports");

            entity.HasOne(d => d.AirportTo).WithMany(p => p.FlightAirportTos)
                .HasForeignKey(d => d.AirportToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_Airports1");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FlightPassengers_1");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK_FlightPassengers_Flights");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("FK_FlightPassengers_Passengers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
