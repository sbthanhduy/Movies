﻿namespace Movies.API.Auth
{
	public static class IdentityExtentions
	{
		public static Guid? GetUserId(this HttpContext context)
		{
			var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");
			if (Guid.TryParse(userId?.Value, out Guid result))
			{
				return result;
			}
			return null;
		}
	}
}
