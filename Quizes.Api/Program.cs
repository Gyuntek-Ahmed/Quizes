using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quizes.Api.Data;
using Quizes.Api.Data.Entities;
using Quizes.Api.Endpoints;
using Quizes.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
    .AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder
    .Services
    .AddDbContext<QuizContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Quiz")));
builder
    .Services
    .AddTransient<AuthService>();
builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration.GetValue<string>("Jwt:Secret");
        var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey!));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = symmetricKey,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
        };
    });
builder
    .Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(p =>
        {
            var allowedOriginsStr = builder.Configuration.GetValue<string>("AllowedOrigins");
            var allowedOrigins = allowedOriginsStr?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            p
            .WithOrigins(allowedOrigins!)
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    });

var app = builder.Build();
#if DEBUG
ApplyDbMigrations(app.Services);
#endif
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.MapAuthEndpoints();

app.Run();

static void ApplyDbMigrations(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizContext>();

    if (dbContext.Database.GetPendingMigrations().Any())
        dbContext.Database.Migrate();
}