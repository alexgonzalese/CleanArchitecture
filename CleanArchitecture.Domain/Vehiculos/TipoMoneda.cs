namespace CleanArchitecture.Domain.Vehiculos;

public record TipoMoneda
{
    public static TipoMoneda Usd => new(Constans.Usd);
    public static TipoMoneda Eur => new(Constans.Eur);
    public static TipoMoneda None = new(Constans.None);
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