using FluentValidation;
using Microsoft.VisualBasic;
using Movies.Application.DTOs;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Validators;
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
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

		private readonly IMovieRepository _movieRepository;
		private readonly IGenreRepository _genreRepository;
		private readonly IRatingRepository _ratingRepository;
		private readonly IValidator<Movie> _movieValidator;

		public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IGenreRepository genreRepository, IRatingRepository ratingRepository)
		{
			_movieRepository = movieRepository;
			_genreRepository = genreRepository;
			_movieValidator = movieValidator;
			_ratingRepository = ratingRepository;
		}

		public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
		{
			//await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);

			return await _movieRepository.CreateAsync(movie, token);
		}

		public Task<bool> DeleteByIdAsync(int id, CancellationToken token = default)
		{
			return _movieRepository.DeleteByIdAsync(id, token);
		}

		public async Task<IEnumerable<MovieDto>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
		{
			var movies = await _movieRepository.GetAllAsync(userId, token);

			var movieTasks = movies.Select(async x =>
			{
				await Semaphore.WaitAsync(token);
				try
				{
					var userRating = userId is null ? null : await _ratingRepository.GetUserRatingAsync(x.Id, userId.Value, token);
					var rating = await _ratingRepository.GetRatingAsync(x.Id, token);

					return new MovieDto
					{
						Id = x.Id,
						Title = x.Title,
						YearOfRelease = x.YearOfRelease,
						Slug = x.Slug,
						UserRating = userRating,
						Rating = rating
					};
				}
				finally
				{
					Semaphore.Release();
				}
			});

			return await Task.WhenAll(movieTasks);
		}

		public async Task<MovieDto?> GetByIdAsync(int id, Guid? userId = default, CancellationToken token = default)
		{
			var movie = await _movieRepository.GetByIdAsync(id, userId, token);
			if (movie is null) return null;

			var userRating = userId is null ? default : await _ratingRepository.GetUserRatingAsync(id, userId.Value, token);
			var rating = await _ratingRepository.GetRatingAsync(id, token);

			return new MovieDto
			{
				Id = movie.Id,
				Title = movie.Title,
				YearOfRelease = movie.YearOfRelease,
				Slug = movie.Slug,
				UserRating = userRating,
				Rating = rating
			};
		}

		public async Task<MovieDto?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
		{
			var movie = await _movieRepository.GetBySlugAsync(slug, userId, token);
			if (movie is null) return null;

			var userRating = userId is null ? default : await _ratingRepository.GetUserRatingAsync(movie.Id, userId.Value, token);
			var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);

			return new MovieDto
			{
				Id = movie.Id,
				Title = movie.Title,
				YearOfRelease = movie.YearOfRelease,
				Slug = movie.Slug,
				UserRating = userRating,
				Rating = rating
			};
		}

		public async Task<MovieDto?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
		{
			//await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);

			var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);
			if (!movieExists)
			{
				return null;
			}

			await _movieRepository.UpdateAsync(movie, userId, token);

			var userRating = userId is null ? default : await _ratingRepository.GetUserRatingAsync(movie.Id, userId.Value, token);
			var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);

			return new MovieDto
			{
				Id = movie.Id,
				Title = movie.Title,
				YearOfRelease = movie.YearOfRelease,
				Slug = movie.Slug,
				UserRating = userRating,
				Rating = rating
			};
		}

		public async Task<Genre?> GetGenreByNameAsync(string name)
		{
			return await _genreRepository.GetByNameAsync(name);
		}
	}
}
