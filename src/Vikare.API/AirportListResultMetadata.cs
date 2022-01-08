public record AirportListResultMetadata
{
    public int Page { get; init; } = default!;
    public int PageSize { get; init; } = default!;
    public int Total { get; init; } = default!;
}