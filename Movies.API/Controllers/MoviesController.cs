using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Controllers
{

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
		public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
		{
			Movie movie = await request.MapToMovie(_movieService.GetGenreByNameAsync);

			await _movieService.CreateAsync(movie, token);

			var response = movie.MapToResponse();

			return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
		}

		[HttpGet(ApiEndpoints.Movies.Get)]
		public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
		{
			var userId = HttpContext.GetUserId();

			var movie = int.TryParse(idOrSlug, out var id) ?
				await _movieService.GetByIdAsync(id, userId, token) :
				await _movieService.GetBySlugAsync(idOrSlug, userId, token);
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
			var userId = HttpContext.GetUserId();

			var movies = await _movieService.GetAllAsync(userId, token);
			return Ok(movies);
		}

		[Authorize("Admin")]
		[HttpPut(ApiEndpoints.Movies.Update)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMovieRequest request
			, CancellationToken token)
		{
			var userId = HttpContext.GetUserId();

			var movie = await request.MapToMovie(id, _movieService.GetGenreByNameAsync);

			var updatedMovie = await _movieService.UpdateAsync(movie, userId, token);
			if (updatedMovie == null)
			{
				return NotFound();
			}

			var response = updatedMovie.MapToResponse();
			return Ok(response);
		}

		[Authorize("Admin")]
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
