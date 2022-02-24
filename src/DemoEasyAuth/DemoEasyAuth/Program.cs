using DemoEasyAuth.Models;
using Newtonsoft.Json;
using SpectoLogic.Identity.EasyAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Easy Auth Library Auth Handler
builder.Services.UseEasyAuthB2C(op =>
{
    op.TokenFactory = (json) => JsonConvert.DeserializeObject<CustomB2CToken>(json);
    op.ClaimsPrincipalFactory = (identity) => new CustomClaimsPrincipal(identity);
#if DEBUG
    op.IsDevelopmentEnvironment = true;
#else
    op.IsDevelopmentEnvironment = false;
#endif
});

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

// Add the following line to get Authentication with AADB2C
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
