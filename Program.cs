using Microsoft.EntityFrameworkCore;
using csharp_blog_backend.Models;

var builder = WebApplication.CreateBuilder(args);


// MODIFICA AGGIUNTA
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        });
});
// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<BlogContext>(opt =>
//    opt.UseInMemoryDatabase("posts"));

string sConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogContext>(options =>
  options.UseSqlServer(sConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

using (var scope = app.Services.CreateScope())

{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BlogContext>();

    context.Database.EnsureCreated();  //crea il Db

    //DbInitializer.Initialize(context);  //popola il Db creato da context
}

app.Run();
