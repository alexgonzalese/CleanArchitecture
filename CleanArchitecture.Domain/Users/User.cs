using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public sealed class User : Entity
{
    public User(Guid id) : base(id)
    {
    }
}