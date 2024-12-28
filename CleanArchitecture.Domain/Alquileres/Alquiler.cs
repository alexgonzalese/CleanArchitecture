using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class Alquiler : Entity
{
    private Alquiler(
        Guid id,
        Guid vehiculoId,
        Guid userId,
        Moneda? precioPorPeriodo,
        Moneda? mantenimeinto,
        Moneda? accesorios,
        Moneda? precioTotal,
        AlquilerStatus status,
        DateRange? duracion,
        DateTime? fechaCreacion
    ) : base(id)
    {
        VehiculoId = vehiculoId;
        UserId = userId;
        PrecioPorPeriodo = precioPorPeriodo;
        Mantenimeinto = mantenimeinto;
        Accesorios = accesorios;
        PrecioTotal = precioTotal;
        Status = status;
        Duracion = duracion;
        FechaCreacion = fechaCreacion;
    }

    public Guid VehiculoId { get; private set; }
    public Guid UserId { get; private set; }
    public Moneda? PrecioPorPeriodo { get; private set; }
    public Moneda? Mantenimeinto { get; private set; }
    public Moneda? Accesorios { get; private set; }
    public Moneda? PrecioTotal { get; private set; }
    public AlquilerStatus Status { get; private set; }
    public DateRange? Duracion { get; private set; }
    public DateTime? FechaCreacion { get; private set; }
    public DateTime? FechaConfirmacion { get; private set; }
    public DateTime? FechaNegacion { get; private set; }
    public DateTime? FechaCompletado { get; private set; }
    public DateTime? FechaCancelacion { get; private set; }

    public static Alquiler Reservar(
        Vehiculo vehiculo,
        Guid userId,
        DateRange? duracion,
        DateTime? fechaCreacion,
        PrecioService precioService
    )
    {
        var precioDetalle = precioService.CalcularPrecio(vehiculo, duracion);

        var alquiler = new Alquiler(
            Guid.NewGuid(),
            vehiculo.Id,
            userId,
            precioDetalle.PrecioPorPeriodo,
            precioDetalle.Mantenimeinto,
            precioDetalle.Accesorios,
            precioDetalle.PrecioTotal,
            AlquilerStatus.Reservado,
            duracion,
            fechaCreacion
        );

        alquiler.RaiseDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id));

        vehiculo.FechaUltimaAlquiler = fechaCreacion;

        return alquiler;
    }

    public Result Confirmar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Confirmado;
        FechaConfirmacion = utcNow;

        RaiseDomainEvent(new AlquilerConfirmadoDomainEvent(Id));

        return Result.Success();
    }

    public Result Negar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Negado;
        FechaNegacion = utcNow;

        RaiseDomainEvent(new AlquilerNegadoDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancelar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            return Result.Failure(AlquilerErrors.NotConfirmed);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);

        if (currentDate > Duracion!.Inicio)
        {
            return Result.Failure(AlquilerErrors.AlreadyStarted);
        }

        Status = AlquilerStatus.Cancelado;
        FechaCancelacion = utcNow;

        RaiseDomainEvent(new AlquilerCanceladoDomainEvent(Id));

        return Result.Success();
    }
}