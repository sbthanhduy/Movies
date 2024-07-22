using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers
{

	[ApiController]
	public class RatingController : ControllerBase
	{
		private readonly IRatingService _ratingService;

		public RatingController(IRatingService ratingService)
		{
			_ratingService = ratingService;
		}

		[Authorize]
		[HttpPut(ApiEndpoints.Movies.Rate)]
		public async Task<IActionResult> RateMovie([FromRoute] int id, [FromBody] RateMovieRequest request, CancellationToken token)
		{
			var userid = HttpContext.GetUserId();
			var result = await _ratingService.RateMovieAsync(id, request.Rating, userid!.Value, token);
			return result ? Ok() : NotFound();
		}

	}
}
