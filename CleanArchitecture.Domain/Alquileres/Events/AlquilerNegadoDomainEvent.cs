using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres.Events;

public sealed record AlquilerNegadoDomainEvent(Guid AlquilerId) : IDomainEvent;