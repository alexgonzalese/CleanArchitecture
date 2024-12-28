using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public static class AlquilerErrors
{
    public static Error NotFound = new Error(
        "Alquiler.NotFound",
        "El alquiler no fue encontrado."
    );

    public static Error Overlap = new Error(
        "Alquiler.Overlap",
        "El alquiler se superpone con otro alquiler."
    );

    public static Error NotReserved = new Error(
        "Alquiler.NotReserved",
        "El alquiler no está reservado."
    );

    public static Error NotConfirmed = new Error(
        "Alquiler.NotConfirmed",
        "El alquiler no está confirmado."
    );

    public static Error AlreadyStarted = new Error(
        "Alquiler.AlreadyStarted",
        "El alquiler ya ha comenzado."
    );
}