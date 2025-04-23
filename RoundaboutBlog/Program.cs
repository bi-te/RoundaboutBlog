using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Data;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
    opts.LowercaseQueryStrings = true;
    opts.AppendTrailingSlash = true;
});

var mvcBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

string? connectionString = builder.Configuration.GetConnectionString("dbConnection");
if (connectionString is null)
{
    throw new InvalidOperationException("Connection string 'dbConnection' not found.");
}

builder.Services.AddDbContext<AppDbContext>(dbOpts =>
    dbOpts.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddScoped<PostsService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();