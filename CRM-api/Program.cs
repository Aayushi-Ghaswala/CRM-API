using CRM_api.Services.ServicesDepedancy;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.InjectServiceDependecy(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["App:CorsOrigins"]
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.TrimEnd().TrimEnd('/'))
                .ToArray())
              .SetIsOriginAllowedToAllowWildcardSubdomains()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithExposedHeaders("WWW-Authenticate");
    });
});

var app = builder.Build();

app.UseCors();

app.Use(async (context, next) =>
{
    var headers = context.Request.Headers["Origin"];
    var allowedOrigins = builder.Configuration["App:CorsOrigins"].Split(',').ToList();

    // Only apply CORS if the request includes an origin header
    if (headers.Count > 0 && allowedOrigins.Any(x => x.Contains(headers)))
    {
        await next.Invoke();
    }
    else
    {
        context.Response.StatusCode = 403;
    }
});

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
