using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews.Events;

public static class ReviewErrors
{
    public static Error NotEligibleForReview => new("review", "Not eligible for review");
}