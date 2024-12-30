using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public sealed record AlquilerCompletadoDomainEvent(Guid AlquilerId) : IDomainEvent;