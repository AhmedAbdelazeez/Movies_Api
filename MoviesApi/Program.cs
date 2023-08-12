using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MoviesApi.Models;
using MoviesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IGenresService, GenresService>();
builder.Services.AddTransient<IMoviesSevices, MovieService>();
builder.Services.AddAutoMapper(typeof(Program));
var connectionstring = builder.Configuration.GetConnectionString("DefualtConnection");

builder.Services.AddDbContext<ApplicationDbContext>(option => option
.UseSqlServer(connectionstring));

builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "MoviesApi",
            Description = "My first api",
            TermsOfService = new Uri("https://www.google.com"),
            Contact = new OpenApiContact
            {
                Name = "Ahmed",
                Email = "ahmedabdelazeez85@domain.com",
                Url = new Uri("https://www.google.com")
            },
            License = new OpenApiLicense
            {
                Name = "My license",
                Url = new Uri("https://www.google.com")
            }
        });
    // Ïå á end points ßá Çá  
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer",
        BearerFormat="Jwt",
        In = ParameterLocation.Header,

        Description = "Enter You Jwt"

    });
     // æÍÏå end points  Çá
    options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Name="Bearer",
                In =ParameterLocation.Header
            },
            new List<string>()
        }
    }); ;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();           
