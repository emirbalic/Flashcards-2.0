using Microsoft.EntityFrameworkCore;
using VocabularyTrainer.Data.Data;

using VocabularyTrainer.Service.Services;
using VocabularyTrainer.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register FlashcardService for DI
builder.Services.AddScoped<FlashcardService>();

// Register the repository if needed (example)
builder.Services.AddScoped<FlashcardRepository>();

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var corsPolicy = "_myCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Allow frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});



var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
