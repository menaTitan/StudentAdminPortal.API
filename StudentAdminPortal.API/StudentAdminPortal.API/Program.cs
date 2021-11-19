using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.API.Utility;

var builder = WebApplication.CreateBuilder(args);
//adding connection string for appsettings.json 
var connectionString = builder.Configuration.GetConnectionString("StudneAdminPortalDb");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors((opitions) =>
{
    opitions.AddPolicy("angularApplication", (builder) =>
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyHeader().WithMethods("GET", "POST", "PUT", "DELETE")
    .WithExposedHeaders("*"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IStudentRepository, SqlStudentRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("angularApplication");
app.UseAuthorization();

app.MapControllers();

app.Run();
