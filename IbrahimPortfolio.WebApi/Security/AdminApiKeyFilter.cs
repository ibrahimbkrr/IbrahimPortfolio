using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IbrahimPortfolio.WebApi.Security;

// Public portfolio reads and contact submissions remain anonymous.
public sealed class AdminApiKeyFilter(IConfiguration configuration) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        var isMessage = string.Equals(context.RouteData.Values["controller"]?.ToString(), "Message", StringComparison.OrdinalIgnoreCase);
        if ((!isMessage && (HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method)))
            || (isMessage && HttpMethods.IsPost(request.Method))) return;

        var expected = configuration["ApiSettings:AdminApiKey"];
        var supplied = request.Headers["X-Admin-Api-Key"].ToString();
        if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(supplied)
            || !CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(expected)),
                SHA256.HashData(Encoding.UTF8.GetBytes(supplied))))
            context.Result = new UnauthorizedResult();
    }
}
