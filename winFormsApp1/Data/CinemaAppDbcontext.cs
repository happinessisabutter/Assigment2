using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Data
{
    public class CinemaAppDbcontext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Showing> Showings { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLlocalDB;Database=CinemaAppDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Can not delete a movie if it has showings
            modelBuilder.Entity<Showing>()
                        .HasOne(s => s.Movie)
                        .WithMany(m => m.Showings)
                        .HasForeignKey(s => s.MovieId)
                        .OnDelete(DeleteBehavior.Restrict); 

            //Can not delete a room if it has showings
            modelBuilder.Entity<Showing>()
                        .HasOne(s => s.Room)
                        .WithMany(r => r.Showings)
                        .HasForeignKey(s => s.RoomId)
                        .OnDelete(DeleteBehavior.Restrict);

            //Delete all seats when a room is deleted
            modelBuilder.Entity<Room>()
                        .HasMany(r => r.Seats)
                        .WithOne(s => s.Room)
                        .HasForeignKey(s => s.RoomId)
                        .OnDelete(DeleteBehavior.Cascade); 

            //delete all tickets when a showing is deleted
            modelBuilder.Entity<Showing>()
                        .HasMany(s => s.Tickets)
                        .WithOne(t => t.Showing)
                        .HasForeignKey(t => t.ShowingId)
                        .OnDelete(DeleteBehavior.Cascade);

            //can not delete a seat if it has tickets
            modelBuilder.Entity<Ticket>()
                        .HasOne(t => t.Seat)
                        .WithMany(s => s.Tickets)
                        .HasForeignKey(t => t.SeatId)
                        .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
