using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

internal sealed class ReservarAlquilerCommandHandler : ICommandHandler<ReservarAlquilerCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IAlquilerRepository _alquilerRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly PrecioService _precioService;
    private readonly IUnitOfWork _unitOfWork;

    public ReservarAlquilerCommandHandler(
        IUserRepository userRepository,
        IAlquilerRepository alquilerRepository,
        IVehiculoRepository vehiculoRepository,
        PrecioService precioService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _alquilerRepository = alquilerRepository;
        _vehiculoRepository = vehiculoRepository;
        _precioService = precioService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ReservarAlquilerCommand request, CancellationToken cancellationToken)
    {
        //Todo: Extraer las validaciones a otro metodo
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErros.NotFound);
        }

        var vehiculo = await _vehiculoRepository.GetByIdAsync(request.VehiculoId, cancellationToken);

        if (vehiculo is null)
        {
            return Result.Failure<Guid>(VehiculoErrors.NotFound);
        }

        var duracion = DateRange.Create(request.FechaInicio, request.FechaFin);

        if (await _alquilerRepository.IsOverlappingAsync(vehiculo, duracion, cancellationToken))
        {
            return Result.Failure<Guid>(AlquilerErrors.Overlap);
        }

        var alquiler = Alquiler.Reservar(
            vehiculo,
            user.Id,
            duracion,
            DateTime.UtcNow,
            _precioService);

        await _alquilerRepository.Add(alquiler);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return alquiler.Id;
    }
}