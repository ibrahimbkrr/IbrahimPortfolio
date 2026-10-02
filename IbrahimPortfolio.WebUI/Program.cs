using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiRequestState>();
var apiUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:BaseUrl yapılandırılmalıdır.");
if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out var apiUri) || (apiUri.Scheme != "http" && apiUri.Scheme != "https"))
    throw new InvalidOperationException("ApiSettings:BaseUrl geçerli bir HTTP/HTTPS adresi olmalıdır.");
if (!builder.Environment.IsDevelopment() && apiUri.Scheme != "https")
    throw new InvalidOperationException("Production API bağlantısı HTTPS kullanmalıdır.");
void ConfigureApiClient(HttpClient client)
{
    client.BaseAddress = new Uri(apiUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(10);
}

builder.Services.AddHttpClient<IAboutApiService, AboutApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<IExperienceApiService, ExperienceApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<ISkillApiService, SkillApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<IMessageApiService, MessageApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<ISocialMediaApiService, SocialMediaApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<IEducationApiService, EducationApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<IProjectApiService, ProjectApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<ICertificateApiService, CertificateApiService>(ConfigureApiClient);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Home/StatusCodePage/403";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("tr-TR").AddSupportedCultures("tr-TR").AddSupportedUICultures("tr-TR"));
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage/{0}");

if (!app.Environment.IsDevelopment())
{

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();