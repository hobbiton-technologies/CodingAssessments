using CodingAssessment;
using CodingAssessment.Users;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

//Mapster configuration
TypeAdapterConfig.GlobalSettings.Default
    .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
    .PreserveReference(true)
    .IgnoreNullValues(true);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .Where(entry => entry.Errors.Count > 0)
            .SelectMany(entry => entry.Errors)
            .Select(error => error.ErrorMessage);

        return new BadRequestObjectResult(new
        {
            Status = 400,
            Message = errors.FirstOrDefault(),
        });
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Travel Product API",
            Description =
                "A coding assessment for Travel Product API " +
                "\n\nIf you want to have unique packages specific to your account, you can use the user endpoints to generate an API key that you can attach to every request to add or retrieve the packages." +
                "\nBy generating a unique API key and attaching it to every request to add or retrieve your packages, you can ensure that only you have access to them and no one else can edit or delete them." +
                "\n\nTo attach your API key in the authorization header of a request, you will need to include it as a value for the 'Authorization' key in the headers of the request. " +
                "\n\nExample: 'Authorization: your API key'",

            Contact = new OpenApiContact
            {
                Name = "Luckson,Daniel,Vincent",
                Email = "tech@hobbiton.co.zm"
            },
        });
});

builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddNpgsql<PostgresDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();


app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSwagger();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseReDoc(c => c.RoutePrefix = string.Empty);
}

app.UseExceptionHandler(a => a.Run(async context =>
{
    var response = context.Response;
    response.ContentType = "application/json";
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;

    await context.Response.WriteAsJsonAsync(new
    {
        Status = 500,
        Message = exception?.Message ?? "Internal Server Error",
    });
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapPost("upload", async (IFormFile file) =>
    {
        var package = await PackageService.UploadDocumentAsync(file);

        return Results.Ok(new
        {
            Url = package,
        });
    })
    .WithOpenApi(operation =>
    {
        operation.Summary = "Upload a package";
        return operation;
    })
    .WithTags("Upload");

app.MapGet("seed", async (PostgresDbContext db) => await db.Seed()).ExcludeFromDescription();


app.Run();