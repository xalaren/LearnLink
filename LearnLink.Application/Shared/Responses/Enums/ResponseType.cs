namespace LearnLink.Application.Shared.Responses.Enums;

/// <summary>
/// Enumerates semantic response types used by the application's handlers
/// to describe the outcome of operations.
/// </summary>
public enum ResponseTypes
{
    /// <summary>Operation completed successfully.</summary>
    Succeeded,
    /// <summary>Operation failed due to validation errors.</summary>
    Invalid,
    /// <summary>Operation was forbidden (authorization failure).</summary>
    Forbidden,
    /// <summary>Requested resource was not found.</summary>
    NotFound,
    /// <summary>Operation resulted in a conflict (e.g., unique constraint).</summary>
    Conflict,
    /// <summary>Operation failed due to server error.</summary>
    Failed
}
