﻿using Microsoft.VisualBasic;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
	public class MovieService : IMovieService
	{
		private readonly IMovieRepository _movieRepository;

		public MovieService(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
		}

		public Task<bool> CreateAsync(Movie movie)
		{
			return _movieRepository.CreateAsync(movie);
		}

		public Task<bool> DeleteByIdAsync(int id)
		{
			return _movieRepository.DeleteByIdAsync(id);
		}

		public Task<IEnumerable<Movie>> GetAllAsync()
		{
			return _movieRepository.GetAllAsync();
		}

		public Task<Movie?> GetByIdAsync(int id)
		{
			return _movieRepository.GetByIdAsync(id);
		}

		public Task<Movie?> GetBySlugAsync(string slug)
		{
			return _movieRepository.GetBySlugAsync(slug);
		}

		public async Task<Movie> UpdateAsync(Movie movie)
		{
			var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id);
			if (!movieExists)
			{
				return null;
			}

			await _movieRepository.UpdateAsync(movie);
			return await _movieRepository.GetByIdAsync(movie.Id);
		}
	}
}
