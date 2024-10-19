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
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CinemaAppDb;Trusted_Connection=True;");
        }
    }
}
