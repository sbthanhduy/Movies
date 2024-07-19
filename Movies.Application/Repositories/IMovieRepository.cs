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
		Task<bool> CreateAsync(Movie movie);
		Task<Movie?> GetByIdAsync(int id);
		Task<Movie?> GetBySlugAsync(string slug);
		Task<IEnumerable<Movie>> GetAllAsync();
		Task<bool> UpdateAsync(Movie movie);
		Task<bool> DeleteByIdAsync(int id);
		Task<bool> ExistsByIdAsync(int id);

	}
}
