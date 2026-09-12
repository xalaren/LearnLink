namespace LearnLink.Application.Users.Models;

/// <summary>
/// Request model used to initialize the system user during seeding.
/// </summary>
/// <param name="Nickname">Nickname for the system user.</param>
/// <param name="Name">First name.</param>
/// <param name="Lastname">Last name.</param>
/// <param name="Password">Password for the system user.</param>
public record InitializeSystemUserRequest(string Nickname, string Name, string Lastname, string Password);
