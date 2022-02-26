using DemoMSIdentity.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// SEE HERE ==>
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
// Simple
//.AddMicrosoftIdentityWebApp(builder
//                            .Configuration.GetSection("AzureAdB2C"));
// More Advanced Options
.AddMicrosoftIdentityWebApp(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options);
    options.Events ??= new OpenIdConnectEvents();
    options.Events.OnRedirectToIdentityProvider += async (context) =>
    {
        // Custom Code
        // Don't remove this line
        await Task.CompletedTask.ConfigureAwait(false);
    };
});

builder.Services.AddTransient<IClaimsTransformation, DemoClaimsTransformation>();

builder.Services.AddControllersWithViews();

// SEE HERE ==>
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// SEE HERE ==>
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
