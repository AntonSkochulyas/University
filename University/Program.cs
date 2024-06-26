using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using University.Data;
using University.ExceptionHandlers;
using University.Repositories.Interfaces;
using University.Repositories.Realizations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "University API", Version = "v1" });
    c.EnableAnnotations();
});

// Configure Pomelo MySQL
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))));

// Register repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

var app = builder.Build();

// Put to database random values
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UniversityContext>();
    DbInitializer.Initialize(context);
}

// Configure the HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "University API V1");
    });

    // Use custom exception handler
    app.UseDeveloperExceptionPage();
}
else
{
    // Use a built-in exception handler
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();

app.MapControllers();

app.Run();
