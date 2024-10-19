using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public string? Language { get; set; }
        public string? Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
