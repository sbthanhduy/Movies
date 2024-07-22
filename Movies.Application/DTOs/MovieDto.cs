using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.DTOs
{
	public class MovieDto
	{
		public int Id { get; init; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public int YearOfRelease { get; set; }
		public List<string> Genres { get; set; }
		public float? Rating { get; set; }
		public int? UserRating { get; set; }
	}
}
