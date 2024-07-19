using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.API.Mapping;
using Movies.Application.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.TokenValidationParameters = new TokenValidationParameters()
	{
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
		ValidIssuer = config["Jwt:Issuer"],
		ValidAudience = config["Jwt:Audience"],
		ValidateIssuer = true,
		ValidateAudience = true
	};
});

builder.Services.AddAuthorization(x =>
{
	x.AddPolicy("Admin", p => p.RequireClaim("admin", "true"));
	x.AddPolicy("Trusted", p =>
		p.RequireAssertion(c =>
			c.User.HasClaim(m => m is { Type: "admin", Value: "true" }) ||
			c.User.HasClaim(m => m is { Type: "trusted_menber", Value: "true" })
		)
	);
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
	x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Scheme = "Bearer"
	});
	x.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id="Bearer"
				},
				Scheme = "oauth2",
				Name="Bearer",
				In=ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

builder.Services.AddApplication();
builder.Services.AddDatabase(config["ConnectionStrings:DefaultConnection"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
