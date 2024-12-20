using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(option =>
{
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
.AddGoogle(options =>
{
    var configuration = builder.Configuration;
    options.ClientId = configuration.GetValue<string>("GoogleClientId");
    options.ClientSecret = configuration.GetValue<string>("GoogleClientSecret");
    options.SaveTokens = true;
    options.CallbackPath = "/google-callback";

    options.Events.OnRemoteFailure = context =>
    {
            
        context.Response.Redirect("/profile");
        context.HandleResponse();
        return Task.CompletedTask;
    };
})

.AddFacebook(options =>
{
    var configuration = builder.Configuration;
    options.AppId = configuration.GetValue<string>("FacebookAppId");
    options.AppSecret = configuration.GetValue<string>("FacebookAppSecret");
    options.CallbackPath = "/facebook-callback";
    options.SaveTokens = true;

    options.Scope.Add("email");
    options.Scope.Add("public_profile");
    options.Fields.Add("email");
    options.Fields.Add("name");
    options.Fields.Add("first_name");
    options.Fields.Add("last_name");

    options.Events = new OAuthEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect("/signin");
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
})

.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/signin";
    options.LogoutPath = "/signout";
    options.AccessDeniedPath = "/denied";
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});


builder.Services.AddScoped<IAppAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<IEmailService, AzureEmailService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();


// Register HttpClient
builder.Services.AddHttpClient();

// Register ProductService
builder.Services.AddScoped<ProductService>(); 

// Register ProductRepository
builder.Services.AddScoped<ProductRepository>(); 


var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseSession();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();