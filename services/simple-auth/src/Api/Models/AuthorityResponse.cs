namespace SimpleAuth.Api.Models;

public sealed class AuthorityResponse
{
    public required ICollection<DataItem> Data { get; init; }

    public sealed class DataItem
    {
        public required string Key { get; init; }
    }
}
