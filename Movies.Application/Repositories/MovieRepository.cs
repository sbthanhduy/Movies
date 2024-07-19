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

		public async Task<bool> CreateAsync(Movie movie)
		{

			await _dbContext.Movies.AddAsync(movie);
			return await _dbContext.SaveChangesAsync() > 0;

		}

		public async Task<Movie?> GetByIdAsync(int id)
		{
			return await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Id == id);

		}

		public async Task<Movie?> GetBySlugAsync(string slug)
		{
			return await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Slug == slug);
		}

		public async Task<IEnumerable<Movie>> GetAllAsync()
		{
			return await _dbContext.Movies.Include(m => m.Genres).ToListAsync();

		}

		public async Task<bool> UpdateAsync(Movie movie)
		{
			var movieFromDb = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Id == movie.Id);

			movieFromDb.Title = movie.Title;
			movieFromDb.YearOfRelease = movie.YearOfRelease;

			movie.Genres.Clear();
			foreach (var genre in movie.Genres)
			{
				var existingGenre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == genre.Id);
				if (existingGenre != null)
				{
					movieFromDb.Genres.Add(genre);
				}
				movieFromDb.Genres.Add(genre);
			}

			return await _dbContext.SaveChangesAsync() > 0;

		}

		public async Task<bool> DeleteByIdAsync(int id)
		{
			var movieFromDb = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(x => x.Id == id);

			_dbContext.Movies.Remove(movieFromDb);
			return await _dbContext.SaveChangesAsync() > 0;
		}



		public async Task<bool> ExistsByIdAsync(int id)
		{
			return await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id) != null;
		}
	}
}
