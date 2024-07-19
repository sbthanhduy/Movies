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
	public class GenreRepository : IGenreRepository
	{
		private MoviesDbContext _dbContext;

		public GenreRepository(MoviesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> CreateAsync(Genre genre)
		{
			await _dbContext.Genres.AddAsync(genre);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteByIdAsync(int id)
		{
			var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
			if (genre == null)
			{
				return false;
			}
			_dbContext.Genres.Remove(genre);
			return _dbContext.SaveChanges() > 0;
		}

		public async Task<IEnumerable<Genre>> GetAllAsync()
		{
			return await _dbContext.Genres.ToListAsync();
		}

		public async Task<Genre?> GetByIdAsync(int id)
		{
			return await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Genre?> GetByNameAsync(string name)
		{
			return await _dbContext.Genres.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
		}

		public async Task<bool> UpdateAsync(Genre genre)
		{
			var genreFromDb = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == genre.Id);
			if (genreFromDb == null)
				return false;
			genreFromDb.Name = genre.Name;

			return await _dbContext.SaveChangesAsync() > 0;
		}
	}
}
