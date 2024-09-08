namespace SimpleAuth.Api.Models;

public sealed class AuthorityOptions
{
    public const string Section = "Authority";

    public required string Url { get; init; }

    public required string Secret { get; init; }
}
