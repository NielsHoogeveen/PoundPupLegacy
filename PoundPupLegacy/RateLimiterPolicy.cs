namespace PoundPupLegacy;

public record RateLimiterPolicy(string Name, int PermitLimit, int QueueLimit);
