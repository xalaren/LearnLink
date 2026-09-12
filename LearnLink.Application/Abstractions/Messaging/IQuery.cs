namespace LearnLink.Application.Abstractions.Messaging;

/// <summary>
/// Marker interface representing a query in the application's
/// messaging pipeline. Implementations represent requests that
/// retrieve data or information and do not modify application state.
/// </summary>
public interface IQuery { }
