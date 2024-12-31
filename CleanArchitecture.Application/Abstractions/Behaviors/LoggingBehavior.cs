using CleanArchitecture.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var name = request.GetType().Name;

            _logger.LogInformation("Handling command {CommandName} ({@Command})", name, request);

            var result = await next();

            _logger.LogInformation("Handled command {CommandName} ({@Command})", name, request);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling command {CommandName} ({@Command})", request.GetType().Name, request);
            throw;
        }
    }
}