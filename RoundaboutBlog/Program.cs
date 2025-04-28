using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Data;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;
using RoundaboutBlog.Settings;
using RoundaboutBlog.Authorization.Handlers;
using RoundaboutBlog.Authorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("dbConnection") ?? 
                           throw new InvalidOperationException("Connection string 'dbConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(dbOpts => dbOpts.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
    //opts.LowercaseQueryStrings = true;
    opts.AppendTrailingSlash = true;
});

builder.Services.AddDefaultIdentity<AppUser>(opts =>
    {
        opts.SignIn.RequireConfirmedAccount = true;
        opts.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>();

var mvcBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("PostOwnerPolicy", policyBuilder => policyBuilder.AddRequirements(new IsPostOwnerRequirement()));
});

builder.Services.AddScoped<PostsService>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));

builder.Services.AddScoped<IAuthorizationHandler, IsPostOwnerHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();