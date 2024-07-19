using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Mapping
{
	public static class ContractMapping
	{
		public static Movie MapToMovie(this CreateMovieRequest request)
		{

			return new Movie
			{
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = new List<Genre>()
			};
		}

		public static Movie MapToMovie(this UpdateMovieRequest request, int id)
		{

			return new Movie
			{
				Id = id,
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = new List<Genre>()
			};

		}


		public static MovieResponse MapToResponse(this Movie movie)
		{
			return new MovieResponse
			{
				Id = movie.Id,
				Title = movie.Title,
				Slug = movie.Slug,
				YearOfRelease = movie.YearOfRelease,
				Genres = movie.Genres
			};
			
		}

		public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
		{
			return new MoviesResponse
			{
				Items = movies.Select(MapToResponse)
			};
		}
	}
}
