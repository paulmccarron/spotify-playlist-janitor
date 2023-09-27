using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Jobs;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace SpotifyPlaylistJanitorAPIs
{
    /// <summary>
    /// Startup class need for runtime and contains app setup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {

        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The Configuration.</param>
        /// <param name="hostingEnvironment">The HostEnvironment.</param>
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The ServiceCollection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SpotifyOption>(_configuration.GetSection("Spotify"));
            services.AddRouting();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Spotify:ClientId"],
                        ValidAudience = _configuration["Spotify:ClientId"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Spotify:ClientSecret"] ?? ""))
                    };
                });

            services.AddMvc();

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Spotify Playlist Janitor",
                    Version = $"v1",
                    Description = "Spotify Playlist Janitor back end API"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.<br/> 
                      Enter 'Bearer' [space] and then your token in the text input below.<br/> 
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Enable Swagger examples
                options.ExampleFilters();
            });

            services.AddDbContext<SpotifyPlaylistJanitorDatabaseContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddSingleton<ISpotifyService, SpotifyService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, DatabaseUserService>();
            services.AddSingleton<IPlayingStateService, PlayingStateService>();
            services.AddHostedService<SpotifyPollingService>();

            services.AddHttpContextAccessor();

            services.AddLogging(builder =>
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(_configuration.GetSection("Logging"))
                    .SetMinimumLevel(LogLevel.Information)
            );

            services.AddQuartz(q =>
            {
                q.ScheduleJob<SkippedTrackRemoveJob>(trigger => trigger
                    .WithIdentity("SkippedTrackRemoveJob")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromHours(1)).RepeatForever())
                    .WithDescription("Schedueld job to check for skipped tracks to auto-remove from playlists.")
                );
            });

            // ASP.NET Core hosting
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();

            if (_hostingEnvironment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";

                    // Disable "Try it out" button in production environment
                    options.SupportedSubmitMethods(Array.Empty<Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod>());
                });

                app.UseExceptionHandler("/Auth/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Required to serve files with no extension in the .well-known folder
            var options = new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
            };

            app.UsePathBase(new PathString($"/api"));

            app.UseStaticFiles(options);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}