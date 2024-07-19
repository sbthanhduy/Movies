using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
	public class Genre
	{
		public int Id { get; init; }
		public required string Name { get; set; }
		[JsonIgnore]
        public List<Movie> Movies { get; set; }
    }
}
