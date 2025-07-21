using BookstoreManagementSystem.WebApp;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");

builder.Services.AddDbContext<BookstoreDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("BookstoreDb")));

builder.Services.AddSwaggerGen(x =>
{
  x.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
  x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT"
  });

  // Security requirement setup
  x.AddSecurityRequirement(new OpenApiSecurityRequirement
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
      new string[] {}
    }
  });
    
  x.CustomSchemaIds(y => y.FullName);
  x.DocInclusionPredicate((_, _) => true);
  x.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
});

builder.Services.AddCors();
builder.Services.AddBookstoreManagementSystem();
builder.Services.AddJWT(builder.Configuration);
builder
    .Services.AddMvc(opt =>
    {
        opt.EnableEndpointRouting = false;
    })
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.DefaultIgnoreCondition = System
            .Text
            .Json
            .Serialization
            .JsonIgnoreCondition
            .WhenWritingNull
    );


var app = builder.Build();

app.Services.GetRequiredService<ILoggerFactory>().AddSerilogLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "RealWorld API V1"));
app.UseAuthentication();
app.UseAuthorization();
app.UseMvc();
app.Run();
