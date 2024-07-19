using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public interface IGenreRepository
	{
		Task<bool> CreateAsync(Genre genre);
		Task<Genre?> GetByIdAsync(int id);
		Task<Genre?> GetByNameAsync(string name);
		Task<IEnumerable<Genre>> GetAllAsync();
		Task<bool> UpdateAsync(Genre genre);
		Task<bool> DeleteByIdAsync(int id);
	}
}
