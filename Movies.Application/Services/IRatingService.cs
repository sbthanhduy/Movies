using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
	public interface IRatingService
	{
		Task<bool> RateMovieAsync(int movieId, int rating, Guid userid, CancellationToken token = default);

	}
}
