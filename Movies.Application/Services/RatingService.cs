﻿using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
	public class RatingService : IRatingService
	{
		private readonly IRatingRepository _ratingRepository;
		private readonly IMovieRepository _movieRepository;
		public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
		{
			_ratingRepository = ratingRepository;
			_movieRepository = movieRepository;
		}

		public async Task<bool> RateMovieAsync(int movieId, int rating, Guid userid, CancellationToken token = default)
		{
			if (rating is < 0 or > 5)
			{
				throw new ValidationException(new[]
				{
					new ValidationFailure
					{
						PropertyName = "Rating",
						ErrorMessage = "Rating must be between 1 and 5"
					}
				});
			}

			var existMovie = await _movieRepository.ExistsByIdAsync(movieId, token);
			if (!existMovie)
				return false;

			return await _ratingRepository.RateMovieAsync(movieId, rating, userid, token);

		}
	}
}
