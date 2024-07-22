using Microsoft.EntityFrameworkCore;
using Movies.Application.Data;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public class RatingRepository : IRatingRepository
	{
		private readonly MoviesDbContext _dbContext;

		public RatingRepository(MoviesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<float?> GetRatingAsync(int movieId, CancellationToken token = default)
		{
			var rating = await _dbContext.Ratings
				.Where(x => x.MovieId == movieId)
				.GroupBy(x => x.Ratings)
				.Select(r => r.Average(x => x.Ratings))
				.FirstOrDefaultAsync(token);

			return (float)rating;
		}

		public async Task<(float? Rating, int? UserRating)> GetRatingAsync(int movieId, Guid userId, CancellationToken token = default)
		{
			var averageRating = (float?)await _dbContext.Ratings.Where(x => x.MovieId == movieId)
				.AverageAsync(x => x.Ratings, cancellationToken: token);
			var userRating = await _dbContext.Ratings
				.Where(x => x.MovieId == movieId && x.UserId == userId)
				.Select(x => x.Ratings).
				SingleOrDefaultAsync(cancellationToken: token);
			return (averageRating, userRating);
		}

		public async Task<int?> GetUserRatingAsync(int movieId, Guid userId, CancellationToken token)
		{
			return await _dbContext.Ratings
				.Where(x => x.MovieId == movieId && x.UserId == userId)
				.Select(x => x.Ratings).
				SingleOrDefaultAsync(cancellationToken: token);
		}

		public async Task<bool> RateMovieAsync(int movieId, int rating, Guid userid, CancellationToken token = default)
		{
			var existingRating = await _dbContext.Ratings
				.FirstOrDefaultAsync(x => x.UserId == userid && x.MovieId == movieId, cancellationToken: token);
			if (existingRating != null)
			{
				existingRating.Ratings = rating;
			}
			else
			{
				var newRating = new Rating()
				{
					MovieId = movieId,
					UserId = userid,
					Ratings = rating
				};
				await _dbContext.Ratings.AddAsync(newRating, cancellationToken: token);
			}

			return await _dbContext.SaveChangesAsync(cancellationToken: token) > 0;

		}
	}
}
