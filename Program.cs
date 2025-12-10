using API.Services;
using API.Services.ADO.NET;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICertificatesService, API.Services.ADO.NET.CertificatesService>();
builder.Services.AddTransient<ICommentsService, API.Services.ADO.NET.CommentsService>();
builder.Services.AddTransient<ICoursesService, API.Services.ADO.NET.CoursesService>();
builder.Services.AddTransient<IUsersService, API.Services.ADO.NET.UsersService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
