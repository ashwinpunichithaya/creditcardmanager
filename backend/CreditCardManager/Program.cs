using CreditCardManager.Repositories;
using CreditCardManager.Security;
using CreditCardManager.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AuthRepository>(options => options.UseInMemoryDatabase("UserIdentities"));
builder.Services.AddDbContext<CreditCardRepository>(opt =>
{
    opt.UseInMemoryDatabase("CreditCards");
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddSingleton<CardNumberValidator>();
builder.Services.AddSingleton<DataProtectionManager>();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AuthRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CreditCard Manager API",
        Description = "An ASP.NET Core Web API for managing credit cards",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Ashwin Punichithaya",
            Url = new Uri("https://example.com/contact")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

string[] allowedOrigins = app.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? [];

app.UseCors(p => p.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();