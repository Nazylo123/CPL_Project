using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoesStore.Data;
using ShoesStore.IRepository;
using ShoesStore.Model.Momo;
using ShoesStore.Repository;
using ShoesStore.Services;
using System.Text;
using WebApi.Data;
using WebApi.Models;

namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Configure Momo API options
			builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
			builder.Services.AddScoped<IMomoService, MomoService>();

			// Add DbContext
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddDbContext<AuthDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Configure Identity
			builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;
			})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

			// Register Repositories
			builder.Services.AddScoped<IProdudctRepository, ProductRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IProdudctImageRepository, ProdudctImageRepository>();
			builder.Services.AddScoped<ITokenRepository, TokenRepository>();
			builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
			builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
			builder.Services.AddScoped<IProductSizeStockRepository, ProductSizeStockRepository>();
			builder.Services.AddTransient<IEmailRepository, EmailRepository>();

			// Configure JWT Authentication
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
				});

			// Configure Swagger
			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your token"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			// Add Controllers and CORS
			builder.Services.AddControllers();

			var app = builder.Build();

			// Configure Middleware
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
			app.UseHttpsRedirection();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}