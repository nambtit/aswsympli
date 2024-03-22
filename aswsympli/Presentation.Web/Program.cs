using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Presentation.Web;

var builder = WebApplication.CreateBuilder(args);

//var builder = new ConfigurationBuilder()
//        .SetBasePath(env.ContentRootPath)
//        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//        .AddEnvironmentVariables();

//Configuration = builder.Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.BootstrapInfrastructure();

app.Run();
