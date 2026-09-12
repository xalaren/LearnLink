namespace LearnLink.Application.Abstractions.Messaging;

/// <summary>
/// Marker interface representing a command in the application's
/// messaging pipeline. Implementations represent requests that
/// change state but do not return a value.
/// </summary>
public interface ICommand { }
