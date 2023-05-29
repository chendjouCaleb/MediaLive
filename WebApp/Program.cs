using System.IO;
using MediaLive;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services.AddControllers();
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 512 * 1024 * 1024;
});
services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 512 * 1024 * 1024;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<FileDbContext>(options =>
{
    var connectionStrings = builder.Configuration["Database:MediaLiveConnectionStrings"];
    var version = ServerVersion.AutoDetect(connectionStrings);
    options.UseMySql(connectionStrings, version);
    
    options
        .UseMySql(connectionStrings, version)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .EnableThreadSafetyChecks();
});

services
    .AddMediaLive(options =>
    {
        
    })
    .AddLocalFileStores(options =>
    {
        options.DirectoryPath = "E:/Lab/Drive";
    })
    .AddEntityFrameworkStores<FileDbContext>();



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