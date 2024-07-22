using Movies.Application.DTOs;
using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Mapping
{
	public static class ContractMapping
	{
		public static async Task<Movie> MapToMovie(this CreateMovieRequest request, Func<string, Task<Genre?>> GetGenreByNameAsync)
		{
			var movie = new Movie()
			{
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = new List<Genre>()
			};

			foreach (var genreName in request.Genres)
			{
				var genre = await GetGenreByNameAsync(genreName);
				if (genre == null)
				{
					genre = new Genre()
					{
						Name = genreName
					};
				}
				movie.Genres.Add(genre);
			}

			return movie;
		}

		public static async Task<Movie> MapToMovie(this UpdateMovieRequest request, int id, Func<string, Task<Genre?>> GetGenreByNameAsync)
		{
			var movie = new Movie()
			{
				Id = id,
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = new List<Genre>()
			};

			foreach (var genreName in request.Genres)
			{
				var genre = await GetGenreByNameAsync(genreName);
				if (genre == null)
				{
					genre = new Genre()
					{
						Name = genreName
					};
				}
				movie.Genres.Add(genre);
			}

			return movie;
		}

		public static MovieResponse MapToResponse(this MovieDto movieDto)
		{
			return new MovieResponse
			{
				Id = movieDto.Id,
				Title = movieDto.Title,
				Slug = movieDto.Slug,
				YearOfRelease = movieDto.YearOfRelease,
				Genres = movieDto.Genres
			};

		}

		public static MoviesResponse MapToResponse(this IEnumerable<MovieDto> movieDtos)
		{
			return new MoviesResponse
			{
				Items = movieDtos.Select(MapToResponse)
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
				Genres = movie.Genres.Select(x => x.Name)
			};

		}
	}
}
