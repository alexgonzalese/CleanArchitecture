namespace CleanArchitecture.Domain.Vehiculos;

public record TipoMoneda
{
    private static TipoMoneda Usd => new(Constans.Usd);
    private static TipoMoneda Eur => new(Constans.Eur);
    private static TipoMoneda None = new(Constans.None);
    private TipoMoneda(string codigo) => Codigo = codigo;
    public string? Codigo { get; init; }

    public static readonly IReadOnlyCollection<TipoMoneda> All =
    [
        Usd,
        Eur,
        None
    ];

    public static TipoMoneda FromCodigo(string codigo) => All.FirstOrDefault(x => x.Codigo == codigo) ?? throw new ApplicationException("Moneda no encontrada");
}