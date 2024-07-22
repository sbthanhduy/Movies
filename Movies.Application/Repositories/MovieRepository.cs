using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Movies.Application.Data;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public class MovieRepository : IMovieRepository
	{
		private readonly MoviesDbContext _dbContext;

		public MovieRepository(MoviesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
		{

			await _dbContext.Movies.AddAsync(movie, token);
			return await _dbContext.SaveChangesAsync(token) > 0;

		}

		public async Task<Movie?> GetByIdAsync(int id, Guid? userId = default, CancellationToken token = default)
		{
			return await _dbContext.Movies
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(x => x.Id == id, token);

		}

		public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
		{
			var movies = await _dbContext.Movies
				.Include(m => m.Genres)
				.Include(m => m.Ratings)
				.ToListAsync(token);
			return movies.FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
		}

		public async Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
		{
			return await _dbContext.Movies
				.Include(m => m.Genres)
				.ToListAsync(token);

		}

		public async Task<bool> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
		{
			var movieFromDb = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Id == movie.Id, token);

			movieFromDb.Title = movie.Title;
			movieFromDb.YearOfRelease = movie.YearOfRelease;

			movie.Genres.Clear();
			foreach (var genre in movie.Genres)
			{
				var existingGenre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == genre.Id, token);
				if (existingGenre != null)
				{
					movieFromDb.Genres.Add(genre);
				}
				movieFromDb.Genres.Add(genre);
			}

			return await _dbContext.SaveChangesAsync(token) > 0;

		}

		public async Task<bool> DeleteByIdAsync(int id, CancellationToken token = default)
		{
			var movieFromDb = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Id == id, token);

			_dbContext.Movies.Remove(movieFromDb);
			return await _dbContext.SaveChangesAsync(token) > 0;
		}



		public async Task<bool> ExistsByIdAsync(int id, CancellationToken token = default)
		{
			return await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id, token) != null;
		}

		
	}
}
