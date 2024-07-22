namespace Movies.API
{
	public static class ApiEndpoints
	{
		private const string ApiBase = "api";

		public static class Movies
		{
			private const string Base = $"{ApiBase}/movies";

			public const string Create = Base;
			public const string Get = $"{Base}/{{idOrSlug}}";
			public const string GetAll = Base;
			public const string Update = $"{Base}/{{id:int}}";
			public const string Delete = $"{Base}/{{id:int}}";

			public const string Rate = $"{Base}/{{id:int}}/ratings";
			public const string DeleteRating = $"{Base}/{{id:int}}/ratings";

		}

		public static class Ratings
		{
			private const string Base = $"{ApiBase}/ratings";
			private const string GetUserRating = $"{Base}/me";
		}
	}
}
