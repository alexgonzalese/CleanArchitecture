namespace CleanArchitecture.Domain.Vehiculos;

public record Moneda(decimal Monto, TipoMoneda TipoMoneda)

{
public static Moneda operator +(Moneda primero, Moneda segundo)
{
    if (primero.TipoMoneda != segundo.TipoMoneda)
    {
        throw new InvalidOperationException("No se pueden sumar montos de diferentes monedas");
    }

    return new Moneda(primero.Monto + segundo.Monto, primero.TipoMoneda);
}
}