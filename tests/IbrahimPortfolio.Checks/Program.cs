using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using AutoMapper;
using IbrahimPortfolio.Business.Concrete;
using IbrahimPortfolio.Business.Mapping;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.ExperienceDtos;
using IbrahimPortfolio.Dto.MessageDtos;
using IbrahimPortfolio.Dto.ProjectDtos;
using IbrahimPortfolio.Dto.SkillDtos;
using IbrahimPortfolio.Dto.Validation;
using IbrahimPortfolio.Entity.Entities;
using IbrahimPortfolio.WebApi.Security;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using IbrahimPortfolio.DataAccess.Context;
using IbrahimPortfolio.DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;

if (args.Contains("--serve-api"))
{
    // Real API controllers/managers with isolated in-memory repositories; never touches SQL Server.
    var builder = WebApplication.CreateBuilder(args.Where(x => x != "--serve-api").ToArray());
    builder.Services.AddControllers(o => o.Filters.Add<AdminApiKeyFilter>())
        .AddApplicationPart(typeof(IbrahimPortfolio.WebApi.Controllers.MessageController).Assembly);
    builder.Services.AddSingleton(typeof(IGenericRepository<>), typeof(MemoryRepository<>));
    builder.Services.AddAutoMapper(c => c.AddProfile<GeneralMapping>());
    foreach (var type in typeof(MessageManager).Assembly.GetTypes().Where(t => t.Name.EndsWith("Manager")))
        foreach (var contract in type.GetInterfaces()) builder.Services.AddScoped(contract, type);
    var app = builder.Build();
    await app.Services.GetRequiredService<IGenericRepository<About>>().CreateAsync(new About
    {
        FirstName = "Portfolio", LastName = "Test", Title = "Developer", Description = "Test profile", Email = "test@example.com"
    });
    app.MapControllers();
    await app.RunAsync();
    return;
}

var checks = 0;
void Check(bool value, string name)
{
    if (!value) throw new Exception(name);
    checks++;
    Console.WriteLine($"PASS {name}");
}
bool Valid(object value) => Validator.TryValidateObject(value, new ValidationContext(value), new List<ValidationResult>(), true);

var mapping = new MapperConfiguration(c => c.AddProfile<GeneralMapping>(), NullLoggerFactory.Instance);
mapping.AssertConfigurationIsValid();
Check(true, "All AutoMapper mappings validate");
var mapper = mapping.CreateMapper();

// Exercise every manager's missing record, create, read, update and delete paths.
foreach (var name in new[] { "About", "Experience", "Education", "Skill", "Project", "Certificate", "SocialMedia" })
{
    var entity = typeof(Message).Assembly.GetTypes().Single(t => t.Name == name);
    var repository = Activator.CreateInstance(typeof(MemoryRepository<>).MakeGenericType(entity))!;
    var managerType = typeof(MessageManager).Assembly.GetTypes().Single(t => t.Name == name + "Manager");
    var manager = Activator.CreateInstance(managerType, repository, mapper)!;
    async Task<object?> Invoke(string method, object arg)
    {
        var task = (Task)managerType.GetMethod(method)!.Invoke(manager, new[] { arg })!;
        await task;
        return task.GetType().GetProperty("Result")?.GetValue(task);
    }
    var createType = typeof(CreateMessageDto).Assembly.GetTypes().Single(t => t.Name == "Create" + name + "Dto");
    var create = Activator.CreateInstance(createType)!;
    Check(await Invoke("GetByIdAsync", 1) == null, name + " missing record");
    await Invoke("CreateAsync", create);
    Check(await Invoke("GetByIdAsync", 1) != null, name + " create/read");
    var updateType = typeof(CreateMessageDto).Assembly.GetTypes().Single(t => t.Name == "Update" + name + "Dto");
    var update = Activator.CreateInstance(updateType)!;
    updateType.GetProperty("Id")!.SetValue(update, 1);
    var field = updateType.GetProperties().First(p => p.PropertyType == typeof(string));
    field.SetValue(update, "Changed");
    Check((bool)(await Invoke("UpdateAsync", update))!, name + " update");
    var result = await Invoke("GetByIdAsync", 1);
    Check((string?)result!.GetType().GetProperty(field.Name)!.GetValue(result) == "Changed", name + " update mapping");
    Check((bool)(await Invoke("DeleteAsync", 1))! && !(bool)(await Invoke("DeleteAsync", 1))!, name + " delete/missing delete");
}

var messages = new MemoryRepository<Message>();
var messageManager = new MessageManager(messages, mapper);
var before = DateTime.Now;
await messageManager.CreateAsync(new CreateMessageDto { Name = "Sender", Email = "sender@example.com", Subject = "Subject", MessageContent = "Original" });
var message = messages.Items.Single();
Check(!message.IsRead && message.CreatedAt >= before, "New message uses server date and unread state");
var originalDate = message.CreatedAt;
Check(await messageManager.MarkAsReadAsync(message.Id), "Mark read succeeds");
Check(message.IsRead && message.Name == "Sender" && message.Email == "sender@example.com" && message.Subject == "Subject" && message.MessageContent == "Original" && message.CreatedAt == originalDate, "Mark read preserves all submitted fields and date");
Check(await messageManager.MarkAsReadAsync(message.Id) && !await messageManager.MarkAsReadAsync(999), "Read is idempotent and missing returns false");
Check(!Valid(new CreateSkillDto { Name = "Skill", Level = 101 }), "Skill range validation");
Check(!Valid(new CreateMessageDto { Name = "A", Email = "bad", Subject = "B", MessageContent = "C" }), "Email validation");
Check(Valid(new CreateProjectDto { Name = "A", Description = "B", Technologies = "C" }), "Optional project URLs may be null");
Check(!Valid(new CreateProjectDto { Name = "A", Description = "B", Technologies = "C", LiveUrl = "javascript:alert(1)" }), "Unsafe URL rejected");
foreach (var culture in new[] { "tr-TR", "en-US" })
{
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
    Check(Valid(new CreateExperienceDto { CompanyName = "A", Position = "B", Description = "C", StartDate = new(2024, 1, 1) }), "Valid date in " + culture);
    Check(Valid(new CreateExperienceDto { CompanyName = "A", Position = "B", Description = "C", StartDate = new(2026, 10, 8), EndDate = new(2026, 10, 23) }), "Reported October dates accepted in " + culture);
    Check(!Valid(new CreateExperienceDto { CompanyName = "A", Position = "B", Description = "C", StartDate = DateTime.MinValue }), "Missing date rejected in " + culture);
    Check(!Valid(new CreateExperienceDto { CompanyName = "A", Position = "B", Description = "C", StartDate = new(2024, 1, 1), EndDate = new(2023, 1, 1) }), "Date order validation in " + culture);
}
Check(SafeUrlAttribute.IsSafe("/cv.pdf", true) && !SafeUrlAttribute.IsSafe("//evil.example", true) && !SafeUrlAttribute.IsSafe("/\\evil.example", true), "Local URL guard");

var key = Guid.NewGuid().ToString("N");
var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["ApiSettings:AdminApiKey"] = key }).Build();
foreach (var controller in new[] { "About", "Experience", "Education", "Skill", "Projects", "Certificate", "SocialMedia", "Message" })
foreach (var method in new[] { "GET", "POST", "PUT", "DELETE" })
foreach (var authorized in new[] { false, true })
{
    var http = new DefaultHttpContext();
    http.Request.Method = method;
    if (authorized) http.Request.Headers["X-Admin-Api-Key"] = key;
    var route = new RouteData(); route.Values["controller"] = controller;
    var filterContext = new AuthorizationFilterContext(new ActionContext(http, route, new ActionDescriptor()), new List<IFilterMetadata>());
    new AdminApiKeyFilter(config).OnAuthorization(filterContext);
    var isPublic = controller == "Message" ? method == "POST" : method == "GET";
    Check((filterContext.Result == null) == (authorized || isPublic), $"API authorization {controller} {method} key={authorized}");
}

var state = new ApiRequestState(NullLogger<ApiRequestState>.Instance, new HttpContextAccessor());
using var client = new HttpClient(new FailureHandler()) { BaseAddress = new Uri("http://localhost/") };
Check(await state.GetAsync<ResultMessageDto>(client, "missing") == null && !state.HasErrors, "404 handled without exception");
Check(await state.GetAsync<ResultMessageDto>(client, "offline") == null && state.HasErrors, "Network failure recorded");
Check(!await state.SendAsync(client, HttpMethod.Post, "offline", new { Name = "Test" }), "Failed POST is not reported as success");
Check(await state.GetAsync<ResultMessageDto>(client, "invalid-json") == null, "Invalid JSON handled");

if (args.Contains("--check-sql"))
{
    var connection = Environment.GetEnvironmentVariable("PORTFOLIO_TEST_SQL")
        ?? throw new InvalidOperationException("Set PORTFOLIO_TEST_SQL explicitly for the SQL check.");
    await using var database = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connection).Options);
    await using var transaction = await database.Database.BeginTransactionAsync();
    try
    {
        var sqlMessages = new MessageManager(new GenericRepository<Message>(database), mapper);
        await sqlMessages.CreateAsync(new CreateMessageDto { Name = "Portfolio automated check", Email = "test@example.com", Subject = "Transactional test", MessageContent = "Rolled back after verification" });
        var created = database.Messages.Local.Single();
        var id = created.Id;
        var date = created.CreatedAt;
        database.ChangeTracker.Clear();
        var persisted = await sqlMessages.GetByIdAsync(id);
        Check(persisted != null && !persisted.IsRead && persisted.CreatedAt == date, "SQL message persists unread with server date");
        Check(await sqlMessages.MarkAsReadAsync(id), "SQL mark read");
        database.ChangeTracker.Clear();
        persisted = await sqlMessages.GetByIdAsync(id);
        Check(persisted?.IsRead == true && persisted.CreatedAt == date && persisted.MessageContent == "Rolled back after verification", "SQL read preserves content/date");
        Check(await sqlMessages.DeleteAsync(id) && await sqlMessages.GetByIdAsync(id) == null, "SQL delete");
    }
    finally { await transaction.RollbackAsync(); }
}
Console.WriteLine($"{checks} checks passed.");

public sealed class MemoryRepository<T> : IGenericRepository<T> where T : class
{
    public List<T> Items { get; } = new();
    public Task<List<T>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<T?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => (int)typeof(T).GetProperty("Id")!.GetValue(x)! == id));
    public Task CreateAsync(T entity) { typeof(T).GetProperty("Id")!.SetValue(entity, Items.Count + 1); Items.Add(entity); return Task.CompletedTask; }
    public void Update(T entity) { }
    public void Delete(T entity) => Items.Remove(entity);
    public Task<int> SaveChangesAsync() => Task.FromResult(1);
}
public sealed class FailureHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => request.RequestUri!.AbsolutePath switch
    {
        "/missing" => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)),
        "/invalid-json" => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("invalid") }),
        _ => throw new HttpRequestException("Simulated offline API")
    };
}
