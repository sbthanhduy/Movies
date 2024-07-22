using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public interface IRatingRepository
	{
		Task<bool> RateMovieAsync(int movieId,int rating, Guid userid, CancellationToken token = default);
		Task<float?> GetRatingAsync(int movieId, CancellationToken token = default);
		Task<int?> GetUserRatingAsync(int movieId, Guid userId, CancellationToken token);
		Task<(float? Rating, int? UserRating)> GetRatingAsync(int movieId, Guid userId, CancellationToken token = default);
	}
}
