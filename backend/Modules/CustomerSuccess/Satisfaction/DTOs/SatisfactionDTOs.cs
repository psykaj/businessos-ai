namespace backend.Modules.CustomerSuccess.Satisfaction.DTOs;

public record CustomerFeedbackDto(
    Guid Id,
    Guid CustomerId,
    string? CustomerName,
    int Rating,
    string? Feedback,
    string Channel,
    string FeedbackType,
    DateTime SubmittedAt
);

public record SubmitFeedbackDto(
    Guid CustomerId,
    int Rating,
    string? Feedback,
    string? Channel,
    string? FeedbackType
);

public record SatisfactionSummaryDto(
    double AverageRating,
    int TotalSubmissions,
    Dictionary<int, int> RatingDistribution,
    double PositiveFeedbackPercentage,
    double NegativeFeedbackPercentage
);
