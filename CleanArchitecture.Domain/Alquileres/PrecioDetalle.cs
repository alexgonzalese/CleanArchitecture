using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public record PrecioDetalle(Moneda PrecioPorPeriodo, Moneda Mantenimeinto, Moneda Accesorios, Moneda PrecioTotal);