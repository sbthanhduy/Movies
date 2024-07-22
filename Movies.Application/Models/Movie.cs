using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
	public partial class Movie
	{
		public int Id { get; init; }
		public required string Title { get; set; }
		public string Slug => GenerateSlug();
		public required int YearOfRelease { get; set; }
		public required List<Genre> Genres { get; set; }

		public List<Rating> Ratings { get; set; }

		private string GenerateSlug()
		{
			var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
				.ToLower().Replace(" ", "-");
			return $"{sluggedTitle}-{YearOfRelease}";
		}

		[GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
		private static partial Regex SlugRegex();
	}
}
