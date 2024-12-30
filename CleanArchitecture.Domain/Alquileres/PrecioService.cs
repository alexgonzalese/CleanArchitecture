using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public class PrecioService
{
    public PrecioDetalle CalcularPrecio(Vehiculo vehiculo, DateRange periodo)
    {
        var tipoMoneda = vehiculo.Precio!.TipoMoneda;
        var precioPorPeriodo = new Moneda(periodo.CantidadDias * vehiculo.Precio.Monto, tipoMoneda);

        decimal porcentageChange = 0;

        foreach (var accesorio in vehiculo.Accesorios)
        {
            porcentageChange += accesorio switch
            {
                Accesorio.Wifi or Accesorio.Gps => 0.1m,
                Accesorio.AsientoNiño => 0.05m,
                _ => 0
            };
        }

        var accesorioCharges = Moneda.Zero(tipoMoneda);

        if (porcentageChange > 0)
        {
            accesorioCharges = new Moneda(precioPorPeriodo.Monto * porcentageChange, tipoMoneda);
        }

        var precioTotal = precioPorPeriodo + accesorioCharges;

        if (!vehiculo!.Mantenimiento!.IsZero())
        {
            precioTotal += vehiculo.Mantenimiento;
        }

        precioTotal += accesorioCharges;

        return new PrecioDetalle(precioPorPeriodo, vehiculo.Mantenimiento, accesorioCharges, precioTotal);
    }
}