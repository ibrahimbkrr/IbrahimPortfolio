using System.ComponentModel.DataAnnotations;

namespace IbrahimPortfolio.Dto.Validation;

public sealed class SafeUrlAttribute : ValidationAttribute
{
    public bool AllowLocal { get; set; }

    public SafeUrlAttribute() : base("Geçerli bir HTTP/HTTPS adresi girin.") { }

    public static bool IsSafe(string? value, bool allowLocal = false)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Any(char.IsControl) || value.Contains('\\'))
            return false;
        if (allowLocal && value.StartsWith('/') && !value.StartsWith("//"))
            return true;
        return Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            && !string.IsNullOrEmpty(uri.Host) && string.IsNullOrEmpty(uri.UserInfo);
    }

    public override bool IsValid(object? value) => value == null || value is string text
        && (text.Length == 0 || IsSafe(text, AllowLocal));
}
