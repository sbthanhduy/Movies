using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Data;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
	public static class ApplicationServiceCollectionExtentions
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddSingleton<IMovieRepository, MovieRepository>();
			services.AddSingleton<IGenreRepository, GenreRepository>();
			services.AddSingleton<IMovieService, MovieService>();
			services.AddValidatorsFromAssemblyContaining<IApplicationMaker>(ServiceLifetime.Singleton);
			return services;
		}

		public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<MoviesDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
			return services;
		}
	}
}
