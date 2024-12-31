using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public static class UserErros
{
    public static Error NotFound = new("User not found", "User not found");
    public static Error InvalidCredentials = new("Invalid credentials", "Invalid credentials");
}