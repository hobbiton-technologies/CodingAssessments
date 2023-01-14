using CodingAssessment;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Travel Product API",
            Description = "A coding assessment for Travel Product API",
            Contact = new OpenApiContact
            {
                Name = "Luckson,Daniel,Vincent",
                Email = "tech@hobbiton.co.zm"
            }
        });
});

builder.Services.AddScoped<PackageService>();
builder.Services.AddNpgsql<PostgresDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();
app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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


app.MapGet("seed", async (PostgresDbContext db) =>
{


    var package = new Package
    {
        Name = "Basic Travel Insurance" ,
        Description = "This is a basic travel insurance package that provides coverage for medical emergencies and trip cancellations.",
        Premium = 50.99,
        SupportingDocumentUrl = "https://www.africau.edu/images/default/sample.pdf",
        Benefits = new List<Benefit>
        {
            new()
            {
                Name = "Medical Emergency Coverage",
                Description = "This benefit provides coverage for medical emergencies that occur while traveling, including hospital stays and emergency medical transportation."
            },
            new()
            {
                Name = "Trip Cancellation Coverage",
                Description = "This benefit provides coverage for trip cancellations due to medical emergencies, natural disasters, and other covered reasons."
            }
            
        }
    };
    
    
    db.Packages.Add(package);

    var users = new List<string> { "Vincent", "Paul", "Mulenga", "Kembo", "Situmbeko" }.Select(x => new User
    {
        UserName = x,
        Email = $"{x}@gmail.com"
    });

    db.Users.AddRange(users);
    
    await db.SaveChangesAsync();
    
    var transactions = Enumerable.Range(1, 500_000).Select(x => new Transaction
    {
        Amount = new Random().Next(1, 1000),
        Date =  RandomDateBetween(new DateTime(2019, 1, 1), new DateTime(2022, 1, 1)).Date,    
        UserId = new Random().Next(1, 5),
        Type = new Random().Next(1, 3) == 1 ? "deposit" : "withdraw",
    });

    await db.Transactions.AddRangeAsync(transactions);
    
    await db.SaveChangesAsync();
    
    
    
    
    
    

    DateTime RandomDateBetween(DateTime start, DateTime end)
    {
        var range = end - start;
        var randTimeSpan = new TimeSpan((long)(new Random().NextDouble() * range.Ticks));
        return start + randTimeSpan;
    }
    
    return "Done";
});

app.Run();