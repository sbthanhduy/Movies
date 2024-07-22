using Movies.Application.DTOs;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
	public interface IMovieService
	{
		Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
		Task<MovieDto?> GetByIdAsync(int id, Guid? userId =default, CancellationToken token = default);
		Task<MovieDto?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);
		Task<IEnumerable<MovieDto>> GetAllAsync(Guid? userId = default, CancellationToken token = default);
		Task<MovieDto> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default);
		Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);
		Task<Genre?> GetGenreByNameAsync(string name);
	}
}
