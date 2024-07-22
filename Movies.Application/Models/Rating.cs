﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
	public class Rating
	{
		public Guid UserId { get; set; }
        public int Ratings { get; set; }
        public int MovieId { get; set; }

		public Movie Movie { get; set; }

	}
}
