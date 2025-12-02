using System;
using System.Collections.Generic;

namespace Feed.Domain.Common;

public class PoolFilter
{
    // Filtering
    public string? Search { get; set; }               // search in Title or Description
    public int? Status { get; set; }                  // filter by status
    public string? CreatedById { get; set; }          // filter by creator
    public DateTime? CreatedFrom { get; set; }        // created from
    public DateTime? CreatedTo { get; set; }          // created to
    public bool IncludeDeleted { get; set; } = false; // include soft deleted items

    // Includes
    public bool IncludeOptions { get; set; } = false;
    public bool IncludeVotes { get; set; } = false;

    // Paging
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    // Sorting
    // allow values: "title", "created", "closes", "status"
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}
