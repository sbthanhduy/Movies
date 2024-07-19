using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Controllers
{
	[Authorize]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IMovieService _movieService;
		private readonly IGenreRepository _genreRepository;

		public MoviesController(IMovieService movieRepository, IGenreRepository genreRepository)
		{
			_movieService = movieRepository;
			_genreRepository = genreRepository;
		}

		[HttpPost(ApiEndpoints.Movies.Create)]
		public async Task<IActionResult> Create([FromBody] CreateMovieRequest request,CancellationToken token)
		{
			var movie = new Movie()
			{
				Title = request.Title,
				YearOfRelease = request.YearOfRelease,
				Genres = new List<Genre>()
			};

			foreach (var genreName in request.Genres)
			{
				var genre = await _genreRepository.GetByNameAsync(genreName);
				if (genre == null)
				{
					genre = new Genre()
					{
						Name = genreName
					};
				}
				movie.Genres.Add(genre);
			}

			await _movieService.CreateAsync(movie, token);

			var response = movie.MapToResponse();

			return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
		}

		[HttpGet(ApiEndpoints.Movies.Get)]
		public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
		{
			var movie = int.TryParse(idOrSlug, out var id) ?
				await _movieService.GetByIdAsync(id, token) :
				await _movieService.GetBySlugAsync(idOrSlug, token);
			if (movie is null)
			{
				return NotFound();
			}
			var response = movie.MapToResponse();
			return Ok(response);
		}

		[HttpGet(ApiEndpoints.Movies.GetAll)]
		public async Task<IActionResult> GetAll(CancellationToken token)
		{
			var movies = await _movieService.GetAllAsync();
			return Ok(movies);
		}

		[HttpPut(ApiEndpoints.Movies.Update)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMovieRequest request
			, CancellationToken token)
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
				var genre = await _genreRepository.GetByNameAsync(genreName);
				if(genre == null)
				{
					genre = new Genre() { Name = genreName };
				}
				movie.Genres.Add(genre);
			}

			var updatedMovie = await _movieService.UpdateAsync(movie, token);
			if (updatedMovie == null)
			{
				return NotFound();
			}

			var response = updatedMovie.MapToResponse();
			return Ok(response);
		}

		[HttpDelete(ApiEndpoints.Movies.Delete)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
		{
			var deleted = await _movieService.DeleteByIdAsync(id, token);
			if (!deleted)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}
