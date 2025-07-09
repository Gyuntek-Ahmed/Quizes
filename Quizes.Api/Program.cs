using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
app.MapAuthEndpoints();

app.Run();

static void ApplyDbMigrations(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizContext>();

    if (dbContext.Database.GetPendingMigrations().Any())
        dbContext.Database.Migrate();
}