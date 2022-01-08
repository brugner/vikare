using System.ComponentModel.DataAnnotations;

public record AirportListParams
{
    public int Page { get; init; } = default!;
    public int PageSize { get; init; } = default!;
    public string? Search { get; init; } = default!;
    public bool ExcludeMetadata { get; init; } = default!;

    public AirportListParams(int? page, int? pageSize, string? search, bool? excludeMetadata)
    {
        if (!page.HasValue || page.Value < 1)
            page = 1;

        if (!pageSize.HasValue || pageSize.Value < 1)
            pageSize = 10;

        if (pageSize > 50)
            pageSize = 50;

        if (!excludeMetadata.HasValue)
            excludeMetadata = false;

        Page = page.Value;
        PageSize = pageSize.Value;
        Search = search;
        ExcludeMetadata = excludeMetadata.Value;
    }
}