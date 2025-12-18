using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICertificatesService, API.Services.EF.CertificatesService>();
builder.Services.AddTransient<ICommentsService, API.Services.EF.CommentsService>();
builder.Services.AddTransient<ICoursesService, API.Services.EF.CoursesService>();
builder.Services.AddTransient<IUsersService, API.Services.EF.UsersService>();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
