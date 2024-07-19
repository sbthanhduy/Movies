using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Responses
{
	public class MovieResponse
	{
        public required int Id { get; init; }
        public required string Title { get; init; }
        public required string Slug { get; init; }
		public required int YearOfRelease { get; init; }
		public required IEnumerable<Genre> Genres { get; init; }
	}
}
