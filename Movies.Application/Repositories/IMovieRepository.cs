﻿using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public interface IMovieRepository
	{
		Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
		Task<Movie?> GetByIdAsync(int id, Guid? userId = default, CancellationToken token = default);
		Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);
		Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default);
		Task<bool> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default);
		Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);
		Task<bool> ExistsByIdAsync(int id, CancellationToken token = default);

	}
}
