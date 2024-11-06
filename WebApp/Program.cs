using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthSection["ClientId"];
        options.ClientSecret = googleAuthSection["ClientSecret"];
        options.SaveTokens = true;
        options.CallbackPath = "/google-callback";

        options.Events.OnRemoteFailure = context =>
        {
            
            context.Response.Redirect("/profile");
            context.HandleResponse();
            return Task.CompletedTask;
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

var app = builder.Build();


app.UseExceptionHandler("/Home/Error");
app.UseSession();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
