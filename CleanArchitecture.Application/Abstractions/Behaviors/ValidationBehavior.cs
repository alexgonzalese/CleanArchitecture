using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class ValidationBehavior<Trequest, TResponse> : IPipelineBehavior<Trequest, TResponse> where Trequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<Trequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<Trequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(Trequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<Trequest>(request);

        var validationErrors = _validators
            .Select(validators => validators.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage)).ToList();

        if (validationErrors.Any())
            throw new Exceptions.ValidationException(validationErrors);

        return await next();
    }
}
//todo: profundizar en ValidationBehavior