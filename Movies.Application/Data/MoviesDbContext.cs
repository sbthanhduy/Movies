using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Data
{
	public class MoviesDbContext : DbContext
	{
		public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
		{

		}

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Genre> Genres { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Movie>()
				.HasMany(e => e.Genres)
				.WithMany(e => e.Movies);
		}

	}
}
