using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Business.Concrete;
using IbrahimPortfolio.Business.Mapping;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.DataAccess.Concrete;
using IbrahimPortfolio.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);

builder.Services.AddScoped<IProjectService, ProjectManager>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<GeneralMapping>();
});

builder.Services.AddScoped<IAboutService, AboutManager>();

builder.Services.AddScoped<IExperienceService, ExperienceManager>();

builder.Services.AddScoped<IEducationService, EducationManager>();

builder.Services.AddScoped<ISkillService, SkillManager>();

builder.Services.AddScoped<ICertificateService, CertificateManager>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
builder.Services.AddScoped<IMessageService, MessageManager>();

builder.Services.AddControllers(options => options.Filters.Add<IbrahimPortfolio.WebApi.Security.AdminApiKeyFilter>());
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();