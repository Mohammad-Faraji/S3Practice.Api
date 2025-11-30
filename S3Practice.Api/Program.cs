using S3Practice.Api.Models.AppSetting;
using S3Practice.Api.Service.Implement;
using S3Practice.Api.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// خواندن تنظیمات S3 از appsettings
builder.Services.Configure<S3Configuration>(builder.Configuration.GetSection("S3Configuration"));

// ثبت سرویس S3 در DI
builder.Services.AddScoped<IFileStorageService, S3FileStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
